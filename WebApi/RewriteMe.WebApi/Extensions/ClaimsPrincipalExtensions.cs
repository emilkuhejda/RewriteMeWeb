using System;
using System.Security.Claims;

namespace RewriteMe.WebApi.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static Guid GetNameIdentifier(this ClaimsPrincipal claimsPrincipal)
        {
            return Guid.Parse(claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier));
        }
    }
}
