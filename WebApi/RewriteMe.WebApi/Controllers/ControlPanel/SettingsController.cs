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
        public async Task<IActionResult> CleanUp(int deleteBeforeInDays, CleanUpSettings cleanUpSettings, bool forceCleanup)
        {
            var deleteBefore = DateTime.UtcNow.AddDays(-deleteBeforeInDays);
            await _cleanUpService.CleanUpAsync(deleteBefore, cleanUpSettings, forceCleanup).ConfigureAwait(false);

            return Ok();
        }
    }
}