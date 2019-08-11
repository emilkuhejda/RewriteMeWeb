using System;
using System.Security.Claims;
using RewriteMe.Domain.Enums;

namespace RewriteMe.WebApi.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static Guid GetNameIdentifier(this ClaimsPrincipal claimsPrincipal)
        {
            return Guid.Parse(claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier));
        }

        public static Role GetRole(this ClaimsPrincipal claimsPrincipal)
        {
            var role = claimsPrincipal.FindFirstValue(ClaimTypes.Role);
            return Enum.Parse<Role>(role);
        }
    }
}
