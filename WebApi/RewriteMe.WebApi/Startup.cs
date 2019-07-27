using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Logging;
using RewriteMe.DataAccess;
using RewriteMe.Domain.Settings;
using RewriteMe.WebApi.Extensions;
using RewriteMe.WebApi.Filters;
using RewriteMe.WebApi.Security;
using RewriteMe.WebApi.Security.Extensions;
using RewriteMe.WebApi.Services;
using Swashbuckle.AspNetCore.Swagger;

namespace RewriteMe.WebApi
{
    public class Startup
    {
        public Startup()
        {
#if DEBUG
            var builder = new ConfigurationBuilder()
                .AddJsonFile("AppSettings.Debug.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();
#else
            var builder = new ConfigurationBuilder()
                .AddJsonFile("AppSettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();
#endif

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            var appSettingsSection = Configuration.GetSection("ApplicationSettings");
            var appSettings = appSettingsSection.Get<AppSettings>();

            services.AddSwaggerGen(configuration =>
            {
                configuration.SwaggerDoc("v1", new Info
                {
                    Title = "Rewrite Me API",
                    Version = "v1"
                });

                configuration.EnableAnnotations();
                configuration.OperationFilter<FormFileSwaggerFilter>();
                configuration.CustomSchemaIds(type =>
                {
                    var returnedValue = type.Name;
                    if (returnedValue.EndsWith("Dto", StringComparison.Ordinal))
                        returnedValue = returnedValue.Replace("Dto", string.Empty, StringComparison.Ordinal);

                    return returnedValue;
                });

                configuration.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    In = "header",
                    Description = "Please enter into field the word 'Bearer' following by space and JWT",
                    Name = "Authorization",
                    Type = "apiKey"
                });

                configuration.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>> { { "Bearer", Enumerable.Empty<string>() } });
            });

            services.AddHangfire(configuration =>
            {
                configuration.UseSqlServerStorage(appSettings.ConnectionString);
            });

            services.Configure<AppSettings>(appSettingsSection);
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(appSettings.ConnectionString, providerOptions => providerOptions.CommandTimeout(60)));

            services.AddRewriteMeAuthorization(appSettings);
            services.AddAzureAdAuthorization(appSettings);
            services.AddMvc().AddFilterProvider((serviceProvider) =>
            {
                var azureAdAuthorizeFilter = new AuthorizeFilter(new[] { new AuthorizeData { AuthenticationSchemes = Constants.AzureAdScheme } });
                var rewriteMeAuthorizeFilter = new AuthorizeFilter(new[] { new AuthorizeData { AuthenticationSchemes = Constants.RewriteMeScheme } });

                var filterProviderOptions = new[]
                {
                    new FilterProviderOption
                    {
                        RoutePrefix = "api",
                        Filter = azureAdAuthorizeFilter
                    },
                    new FilterProviderOption
                    {
                        RoutePrefix = "control-panel",
                        Filter = rewriteMeAuthorizeFilter
                    }
                };

                return new AuthenticationFilterProvider(filterProviderOptions);
            });

            services.AddAuthentication(options =>
                {
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.Audience = appSettings.Authentication.ClientId;
                    options.Authority = $"{appSettings.Authentication.AuthoritySignUpSignIn}/v2.0/";
                });

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            return Bootstrapper.BootstrapRuntime(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                IdentityModelEventSource.ShowPII = true;
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.Use(async (context, next) =>
            {
                await next();

                if (context.Response.StatusCode == 404 &&
                    !Path.HasExtension(context.Request.Path.Value) &&
                    !context.Request.Path.Value.StartsWith("/api/", StringComparison.InvariantCultureIgnoreCase) &&
                    !context.Request.Path.Value.StartsWith("/control-panel/", StringComparison.InvariantCultureIgnoreCase))
                {
                    context.Request.Path = "/home/index.html";
                    await next();
                }
                else if (context.Response.StatusCode == 404 &&
                         !Path.HasExtension(context.Request.Path.Value) &&
                         context.Request.Path.Value.StartsWith("/control-panel/", StringComparison.InvariantCultureIgnoreCase))
                {
                    context.Request.Path = "/control-panel/error";
                    await next();
                }
            });

            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());

            app.Migrate();
            app.ConfigureExceptionMiddleware();
            app.UseCookiePolicy();

            app.UseMvcWithDefaultRoute();
            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Rewrite Me API v1"); });

            app.UseHangfireServer();
            app.UseHangfireDashboard();
            GlobalJobFilters.Filters.Add(new AutomaticRetryAttribute { Attempts = 3, DelaysInSeconds = new[] { 30, 60, 120, 240 } });
        }
    }
}
