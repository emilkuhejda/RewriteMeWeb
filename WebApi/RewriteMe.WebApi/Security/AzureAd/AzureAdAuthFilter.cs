using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace RewriteMe.WebApi.Security.AzureAd
{
    public class AzureAdAuthFilter : AuthorizeFilter
    {
        public AzureAdAuthFilter(IAuthorizationPolicyProvider provider)
            : base(provider, new[] { new AuthorizeData(Constants.AzureAdPolicy) }) { }
    }
}
