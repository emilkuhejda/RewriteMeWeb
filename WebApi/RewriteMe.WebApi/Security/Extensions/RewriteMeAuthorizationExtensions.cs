using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using RewriteMe.Domain.Settings;
using RewriteMe.WebApi.Utils;

namespace RewriteMe.WebApi.Security.Extensions
{
    public static class RewriteMeAuthorizationExtensions
    {
        public static IServiceCollection AddRewriteMeAuthorization(this IServiceCollection services, AppSettings appSettings)
        {
            var issuerSigningKey = Encoding.ASCII.GetBytes(appSettings.SecretKey);

            services
                .AddAuthorization(options => { options.AddRewriteMePolicy(); })
                .AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = Constants.RewriteMeScheme;
                    x.DefaultChallengeScheme = Constants.RewriteMeScheme;
                })
                .AddJwtBearer(Constants.RewriteMeScheme, x =>
                {
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
