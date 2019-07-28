using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.Domain.Settings;

namespace RewriteMe.WebApi.Security.Extensions
{
    public static class RewriteMeAuthorizationExtensions
    {
        public static IServiceCollection AddRewriteMeAuthorization(this IServiceCollection services, AppSettings appSettings)
        {
            var issuerSigningKey = Encoding.ASCII.GetBytes(appSettings.SecretKey);

            services
                .AddAuthorization(options =>
                {
                    options.AddRewriteMePolicy();
                })
                .AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = Constants.RewriteMeScheme;
                    x.DefaultChallengeScheme = Constants.RewriteMeScheme;
                })
                .AddJwtBearer(Constants.RewriteMeScheme, x =>
                {
                    x.Events = new JwtBearerEvents
                    {
                        OnTokenValidated = async context =>
                        {
                            var administratorService = context.HttpContext.RequestServices.GetRequiredService<IAdministratorService>();
                            var administratorId = Guid.Parse(context.Principal.Identity.Name);
                            var administrator = await administratorService.GetAsync(administratorId).ConfigureAwait(false);
                            if (administrator == null)
                            {
                                context.Fail("Unauthorized");
                            }

                            await Task.CompletedTask.ConfigureAwait(false);
                        }
                    };
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(issuerSigningKey),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            return services;
        }

        public static AuthorizationOptions AddRewriteMePolicy(this AuthorizationOptions options)
        {
            var policy = new AuthorizationPolicyBuilder()
                .AddAuthenticationSchemes(Constants.RewriteMeScheme)
                .RequireAuthenticatedUser()
                .Build();

            options.AddPolicy(Constants.RewriteMePolicy, policy);
            return options;
        }
    }
}
