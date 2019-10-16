using System;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RewriteMe.Business.Configuration;
using RewriteMe.Domain.Enums;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.WebApi.Models;

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

        [HttpGet("/api/control-panel/settings/storage-setting")]
        public async Task<IActionResult> GetStorageSetting()
        {
            var readSourceFromDatabase = await _internalValueService.GetValueAsync(InternalValues.ReadSourceFromDatabase).ConfigureAwait(false);
            var storageSetting = readSourceFromDatabase ? StorageSetting.Database : StorageSetting.Disk;

            return Ok(storageSetting);
        }

        [HttpPut("/api/control-panel/settings/change-storage")]
        public async Task<IActionResult> ChangeStorage(StorageSetting storageSetting)
        {
            await _internalValueService.UpdateValueAsync(InternalValues.ReadSourceFromDatabase, storageSetting == StorageSetting.Database).ConfigureAwait(false);

            return Ok();
        }

        [HttpPut("/api/control-panel/settings/clean-up")]
        public IActionResult CleanUp([FromBody]CleanUpSettingsModel cleanUpSettingsModel)
        {
            if (cleanUpSettingsModel.Password != "123456")
                return BadRequest();

            var deleteBefore = DateTime.UtcNow.AddDays(-cleanUpSettingsModel.DeleteBeforeInDays);
            BackgroundJob.Enqueue(() => _cleanUpService.CleanUp(deleteBefore, cleanUpSettingsModel.CleanUpSettings, cleanUpSettingsModel.ForceCleanUp));

            return Ok();
        }
    }
}