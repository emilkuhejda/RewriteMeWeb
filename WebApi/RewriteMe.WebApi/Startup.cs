using System;
using System.Collections.Generic;
using System.IO;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using RewriteMe.Business.Polling;
using RewriteMe.DataAccess;
using RewriteMe.Domain.Settings;
using RewriteMe.WebApi.Extensions;
using RewriteMe.WebApi.Filters;
using RewriteMe.WebApi.Security;
using RewriteMe.WebApi.Security.Extensions;
using RewriteMe.WebApi.Services;
using RewriteMe.WebApi.Utils;
using Serilog;

namespace RewriteMe.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public ILifetimeScope AutofacContainer { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(
                    Constants.CorsPolicy,
                    builder => builder
                        .WithOrigins("http://localhost:4200")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials());
            });

            var appSettingsSection = Configuration.GetSection("ApplicationSettings");
            var appSettings = appSettingsSection.Get<AppSettings>();

            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddApiVersioning();
            services.AddSwaggerGen(configuration =>
            {
                configuration.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Voicipher API",
                    Version = "v1"
                });

                configuration.EnableAnnotations();
                configuration.CustomSchemaIds(type =>
                {
                    var returnedValue = type.Name;
                    if (returnedValue.EndsWith("Dto", StringComparison.Ordinal))
                        returnedValue = returnedValue.Replace("Dto", string.Empty, StringComparison.Ordinal);

                    return returnedValue;
                });

                configuration.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter into field the word 'Bearer' following by space and JWT",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });

                configuration.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Id = "Bearer",
                                Type = ReferenceType.SecurityScheme
                            }
                        },
                        new List<string>()
                    }
                });
            });

            services.AddHangfire(configuration =>
            {
                configuration.UseSqlServerStorage(appSettings.ConnectionString);
            });

            services.Configure<AppSettings>(appSettingsSection);
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(appSettings.ConnectionString, providerOptions => providerOptions.CommandTimeout(60)));

            services.AddSignalR();

            services.AddRewriteMeAuthorization(appSettings);
            services.AddAzureAdAuthorization(appSettings);
            services.AddMvc(c =>
            {
                c.Conventions.Add(new ApiExplorerGroupPerVersionConvention());
                c.EnableEndpointRouting = false;
                c.Filters.AddService<ApiExceptionFilter>();
            }).AddFilterProvider(serviceProvider =>
            {
                var azureAdAuthorizeFilter = new AuthorizeFilter(new[] { new AuthorizeData { AuthenticationSchemes = Constants.AzureAdScheme } });
                var rewriteMeAuthorizeFilter = new AuthorizeFilter(new[] { new AuthorizeData { AuthenticationSchemes = Constants.RewriteMeScheme } });

                var filterProviderOptions = new[]
                {
                    new FilterProviderOption
                    {
                        RoutePrefix = "api/b2c",
                        Filter = azureAdAuthorizeFilter
                    },
                    new FilterProviderOption
                    {
                        RoutePrefix = "api",
                        Filter = rewriteMeAuthorizeFilter
                    }
                };

                return new AuthenticationFilterProvider(filterProviderOptions);
            });

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddOptions();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new ApplicationModule());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            AutofacContainer = app.ApplicationServices.GetAutofacRoot();

            var appSettingsSection = Configuration.GetSection("ApplicationSettings");
            var appSettings = appSettingsSection.Get<AppSettings>();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.Use(async (context, next) =>
            {
                // Eliminates Cross-site Scripting (XSS) Attack
                context.Response.Headers.Add("X-Xss-Protection", "1");

                await next().ConfigureAwait(false);

                if (context.Response.StatusCode == 404 &&
                    !Path.HasExtension(context.Request.Path.Value) &&
                    !context.Request.Path.Value.StartsWith("/api/", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (context.Request.Path.Value.StartsWith("/control-panel/", StringComparison.InvariantCultureIgnoreCase))
                    {
                        context.Request.Path = "/control-panel/index.html";
                    }
                    else if (context.Request.Path.Value.StartsWith("/profile/", StringComparison.InvariantCultureIgnoreCase))
                    {
                        context.Request.Path = "/profile/index.html";
                    }
                    else
                    {
                        context.Request.Path = "/home/index.html";
                    }

                    await next().ConfigureAwait(false);
                }
            });

            app.UseCors(Constants.CorsPolicy);
            app.UseSerilogRequestLogging();

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<CacheHub>("/api/cache-hub");
            });

            app.MigrateDatabase();
            app.UseCookiePolicy();

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Voicipher API v1"));

            app.UseMvcWithDefaultRoute();
            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseHangfireServer();
            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = new[] { new RewriteMeHangfireAuthorizationFilter(appSettings.HangfireSecretKey) }
            });

            GlobalJobFilters.Filters.Add(new AutomaticRetryAttribute { Attempts = 0 });
        }
    }
}
