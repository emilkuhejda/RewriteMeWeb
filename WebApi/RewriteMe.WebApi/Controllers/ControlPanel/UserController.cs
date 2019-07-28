using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RewriteMe.WebApi.Controllers.ControlPanel
{
    [Route("control-panel/[controller]")]
    [Produces("application/json")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpGet("/control-panel/users")]
        public async Task<IActionResult> Get()
        {
            return Ok();
        }
    }
}