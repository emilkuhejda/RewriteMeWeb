using Microsoft.AspNetCore.Mvc;

namespace RewriteMe.WebApi.Security.AzureAd
{
    public sealed class AzureAdAuthFilterAttribute : TypeFilterAttribute
    {
        public AzureAdAuthFilterAttribute()
            : base(typeof(AzureAdAuthFilter)) { }
    }
}
