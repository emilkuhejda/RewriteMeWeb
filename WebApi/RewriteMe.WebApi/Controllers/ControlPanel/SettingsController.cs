using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RewriteMe.Domain.Enums;
using RewriteMe.Domain.Interfaces.Services;

namespace RewriteMe.WebApi.Controllers.ControlPanel
{
    [Produces("application/json")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize(Roles = nameof(Role.Admin))]
    [ApiController]
    public class SettingsController : ControllerBase
    {
        private readonly ICleanUpService _cleanUpService;

        public SettingsController(ICleanUpService cleanUpService)
        {
            _cleanUpService = cleanUpService;
        }

        [HttpGet("/api/control-panel/settings/clean-up")]
        [ApiExplorerSettings(IgnoreApi = false)]
        [AllowAnonymous]
        public async Task<IActionResult> CleanUp(CleanUpSettings cleanUpSettings)
        {
            var deleteBefore = new DateTime(2019, 10, 14, 16, 05, 0);
            cleanUpSettings = CleanUpSettings.Disk | CleanUpSettings.Database;
            await _cleanUpService.CleanAsync(deleteBefore, cleanUpSettings).ConfigureAwait(false);

            return Ok();
        }
    }
}