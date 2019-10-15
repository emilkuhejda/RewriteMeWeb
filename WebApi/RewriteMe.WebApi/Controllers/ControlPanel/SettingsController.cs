using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RewriteMe.Business.Configuration;
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
        private readonly IInternalValueService _internalValueService;
        private readonly ICleanUpService _cleanUpService;

        public SettingsController(
            IInternalValueService internalValueService,
            ICleanUpService cleanUpService)
        {
            _internalValueService = internalValueService;
            _cleanUpService = cleanUpService;
        }

        [HttpGet("/api/control-panel/settings/change-storage")]
        public async Task<IActionResult> ChangeStorage(StorageSetting storageSetting)
        {
            await _internalValueService.UpdateValueAsync(InternalValues.ReadSourceFromDatabase, storageSetting == StorageSetting.Database).ConfigureAwait(false);

            return Ok();
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