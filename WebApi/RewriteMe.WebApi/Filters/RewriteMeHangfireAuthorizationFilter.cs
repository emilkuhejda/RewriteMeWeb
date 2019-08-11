using Hangfire.Dashboard;
using RewriteMe.Domain.Enums;
using RewriteMe.WebApi.Extensions;
using RewriteMe.WebApi.Utils;

namespace RewriteMe.WebApi.Filters
{
    public class RewriteMeHangfireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        private readonly string _secretKey;

        public RewriteMeHangfireAuthorizationFilter(string secretKey)
        {
            _secretKey = secretKey;
        }

        public bool Authorize(DashboardContext context)
        {
            var httpContext = context.GetHttpContext();

            var token = httpContext.Request.Cookies[Constants.HangfireAccessToken];
            if (string.IsNullOrWhiteSpace(token))
                return false;

            var principal = TokenHelper.ValidateToken(_secretKey, token);
            if (principal == null)
                return false;

            var role = principal.GetRole();

            return role == Role.Admin;
        }
    }
}
