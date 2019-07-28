using Microsoft.AspNetCore.Authorization;

namespace RewriteMe.WebApi.Security
{
    public class AuthorizeData : IAuthorizeData
    {
        public AuthorizeData()
        { }

        public AuthorizeData(string policy)
        {
            Policy = policy;
        }

        public string Policy { get; set; }

        public string Roles { get; set; }

        public string AuthenticationSchemes { get; set; }
    }
}
