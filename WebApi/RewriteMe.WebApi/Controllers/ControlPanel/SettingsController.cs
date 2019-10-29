using System;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RewriteMe.Business.Configuration;
using RewriteMe.Domain.Enums;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.Domain.Settings;
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
        private readonly IAuthenticationService _authenticationService;
        private readonly AppSettings _appSettings;

        public SettingsController(
            IInternalValueService internalValueService,
            ICleanUpService cleanUpService,
            IAuthenticationService authenticationService,
            IOptions<AppSettings> options)
        {
            _internalValueService = internalValueService;
            _cleanUpService = cleanUpService;
            _authenticationService = authenticationService;
            _appSettings = options.Value;
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

        [HttpPut("/api/control-panel/settings/change-notifications-setting")]
        public async Task<IActionResult> ChangeNotificationsSettings(bool isEnabled)
        {
            await _internalValueService.UpdateValueAsync(InternalValues.IsProgressNotificationsEnabled, isEnabled).ConfigureAwait(false);

            return Ok();
        }

        [HttpPut("/api/control-panel/settings/clean-up")]
        public IActionResult CleanUp([FromBody]CleanUpSettingsModel cleanUpSettingsModel)
        {
            var passwordHash = _authenticationService.GenerateHash(cleanUpSettingsModel.Password);
            if (passwordHash != _appSettings.SecurityPasswordHash)
                return BadRequest();

            var deleteBefore = DateTime.UtcNow.AddDays(-cleanUpSettingsModel.DeleteBeforeInDays);
            BackgroundJob.Enqueue(() => _cleanUpService.CleanUp(deleteBefore, cleanUpSettingsModel.CleanUpSettings, cleanUpSettingsModel.ForceCleanUp));

            return Ok();
        }
    }
}