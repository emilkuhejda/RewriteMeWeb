using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RewriteMe.Domain.Enums;
using RewriteMe.Domain.Interfaces.Services;

namespace RewriteMe.WebApi.Controllers.ControlPanel
{
    [Produces("application/json")]
    //[ApiExplorerSettings(IgnoreApi = true)]
    //[Authorize(Roles = nameof(Role.Admin))]
    [AllowAnonymous]
    [ApiController]
    public class SettingsController : ControllerBase
    {
        private readonly ICleanUpService _cleanUpService;

        public SettingsController(ICleanUpService cleanUpService)
        {
            _cleanUpService = cleanUpService;
        }

        [HttpGet("/api/control-panel/settings/clean-up")]
        public async Task<IActionResult> CleanUp()
        {
            var deleteBefore = DateTime.UtcNow;
            await _cleanUpService.CleanAsync(deleteBefore).ConfigureAwait(false);

            return Ok();
        }
    }
}