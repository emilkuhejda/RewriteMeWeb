using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace RewriteMe.WebApi.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Authorize]
    [ApiController]
    public class UtilsController : ControllerBase
    {
        [AllowAnonymous]
        [HttpGet("/api/is-alive")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [SwaggerOperation(OperationId = "IsAlive")]
        public IActionResult IsAlive()
        {
            return Ok(true);
        }
    }
}