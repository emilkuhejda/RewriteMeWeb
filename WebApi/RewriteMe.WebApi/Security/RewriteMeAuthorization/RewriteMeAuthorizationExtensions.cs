using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using RewriteMe.Domain.Settings;
using RewriteMe.Domain.UserManagement;

namespace RewriteMe.WebApi.Security.RewriteMeAuthorization
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
                            //var userService = context.HttpContext.RequestServices.GetRequiredService<IUserService>();
                            var userId = Guid.Parse(context.Principal.Identity.Name);
                            var user = new User();
                            if (user == null)
                            {
                                context.Fail("Unauthorized");
                            }

                            await Task.CompletedTask;
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
