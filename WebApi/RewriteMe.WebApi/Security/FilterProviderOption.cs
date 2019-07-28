using Microsoft.AspNetCore.Mvc.Authorization;

namespace RewriteMe.WebApi.Security
{
    public class FilterProviderOption
    {
        public string RoutePrefix { get; set; }

        public AuthorizeFilter Filter { get; set; }
    }
}
