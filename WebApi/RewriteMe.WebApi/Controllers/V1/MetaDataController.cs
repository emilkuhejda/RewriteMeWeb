using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace RewriteMe.WebApi.Controllers.V1
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/meta-data")]
    [Produces("application/json")]
    [ApiController]
    public class MetaDataController : ControllerBase
    {
        [AllowAnonymous]
        [HttpGet(".well-known/openid-configuration")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [SwaggerOperation(OperationId = "Configuration")]
        public IActionResult Configuration()
        {
            return new OkObjectResult(new
            {
                issuer = "https://appleid.apple.com",
                authorization_endpoint = "https://appleid.apple.com/auth/authorize",
                token_endpoint = "https://appleid.apple.com/auth/token",
                jwks_uri = "https://appleid.apple.com/auth/keys",
                id_token_signing_alg_values_supported = new[] { "RS256" }
            });
        }
    }
}
