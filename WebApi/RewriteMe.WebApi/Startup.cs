using System;
using System.IO;
using System.Reflection;
using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RewriteMe.DataAccess;
using RewriteMe.Domain.Settings;
using RewriteMe.WebApi.Extensions;
using RewriteMe.WebApi.Filters;
using RewriteMe.WebApi.Services;
using Swashbuckle.AspNetCore.Swagger;

namespace RewriteMe.WebApi
{
    public class Startup
    {
        public Startup()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("AppSettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

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
                    if (returnedValue.EndsWith("Dto"))
                        returnedValue = returnedValue.Replace("Dto", string.Empty);

                    return returnedValue;
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.XML";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                configuration.IncludeXmlComments(xmlPath);
            });

            services.AddHangfire(configuration =>
            {
                configuration.UseSqlServerStorage(appSettings.ConnectionString);
            });

            services.Configure<AppSettings>(appSettingsSection);
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(appSettings.ConnectionString, providerOptions => providerOptions.CommandTimeout(60)));
            services.AddMvc();

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

            return Bootstrapper.BootstrapRuntime(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
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
                await next();

                if (context.Response.StatusCode == 404 &&
                    !Path.HasExtension(context.Request.Path.Value) &&
                    !context.Request.Path.Value.StartsWith("/api/", StringComparison.InvariantCultureIgnoreCase))
                {
                    context.Request.Path = "/index.html";
                    await next();
                }
            });

            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());

            app.UseAuthentication();
            app.ConfigureExceptionMiddleware();

            app.UseMvcWithDefaultRoute();
            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Rewrite Me API v1"); });

            app.UseHangfireServer();
            app.UseHangfireDashboard();
            GlobalJobFilters.Filters.Add(new AutomaticRetryAttribute { Attempts = 0 });
        }
    }
}
