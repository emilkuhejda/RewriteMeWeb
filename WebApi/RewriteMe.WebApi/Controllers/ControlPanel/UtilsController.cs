using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RewriteMe.Domain.Enums;

namespace RewriteMe.WebApi.Controllers.ControlPanel
{
    [Produces("application/json")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize(Roles = nameof(Role.Admin))]
    [ApiController]
    public class UtilsController : ControllerBase
    {
        [HttpGet("/api/control-panel/has-access")]
        public IActionResult HasAccess()
        {
            return Ok(true);
        }
    }
}