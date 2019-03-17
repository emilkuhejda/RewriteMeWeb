using Microsoft.AspNetCore.Builder;
using RewriteMe.WebApi.Utils;

namespace RewriteMe.WebApi.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static void ConfigureExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ErrorLoggingMiddleware>();
        }
    }
}
