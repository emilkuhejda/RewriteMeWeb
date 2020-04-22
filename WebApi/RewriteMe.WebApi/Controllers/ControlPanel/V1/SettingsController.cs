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

namespace RewriteMe.WebApi.Controllers.ControlPanel.V1
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/control-panel/settings")]
    [Produces("application/json")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize(Roles = nameof(Role.Admin))]
    [ApiController]
    public class SettingsController : ControllerBase
    {
        private readonly IInternalValueService _internalValueService;
        private readonly ICleanUpService _cleanUpService;
        private readonly IAuthenticationService _authenticationService;
        private readonly IUploadedChunkService _uploadedChunkService;
        private readonly IUserSubscriptionService _userSubscriptionService;
        private readonly AppSettings _appSettings;

        public SettingsController(
            IInternalValueService internalValueService,
            ICleanUpService cleanUpService,
            IAuthenticationService authenticationService,
            IUploadedChunkService uploadedChunkService,
            IUserSubscriptionService userSubscriptionService,
            IOptions<AppSettings> options)
        {
            _internalValueService = internalValueService;
            _cleanUpService = cleanUpService;
            _authenticationService = authenticationService;
            _uploadedChunkService = uploadedChunkService;
            _userSubscriptionService = userSubscriptionService;
            _appSettings = options.Value;
        }

        [HttpGet("storage-setting")]
        public async Task<IActionResult> GetStorageSetting()
        {
            var storageSetting = await _internalValueService.GetValueAsync(InternalValues.StorageSetting).ConfigureAwait(false);

            return Ok(storageSetting);
        }

        [HttpGet("database-backup")]
        public async Task<IActionResult> GetDatabaseBackupSetting()
        {
            var isDatabaseBackupEnabled = await _internalValueService.GetValueAsync(InternalValues.IsDatabaseBackupEnabled).ConfigureAwait(false);

            return Ok(isDatabaseBackupEnabled);
        }

        [HttpGet("notifications-setting")]
        public async Task<IActionResult> GetNotificationsSetting()
        {
            var isProgressNotificationsEnabled = await _internalValueService.GetValueAsync(InternalValues.IsProgressNotificationsEnabled).ConfigureAwait(false);

            return Ok(isProgressNotificationsEnabled);
        }

        [HttpGet("chunks-storage-setting")]
        public async Task<IActionResult> GetChunksStorageSetting()
        {
            var chunksStorageSetting = await _internalValueService.GetValueAsync(InternalValues.ChunksStorageSetting).ConfigureAwait(false);

            return Ok(chunksStorageSetting);
        }

        [HttpPut("change-storage")]
        public async Task<IActionResult> ChangeStorage(StorageSetting storageSetting)
        {
            await _internalValueService.UpdateValueAsync(InternalValues.StorageSetting, storageSetting).ConfigureAwait(false);

            return Ok();
        }

        [HttpPut("change-database-backup")]
        public async Task<IActionResult> ChangeDatabaseBackupSettings(bool isEnabled)
        {
            await _internalValueService.UpdateValueAsync(InternalValues.IsDatabaseBackupEnabled, isEnabled).ConfigureAwait(false);

            return Ok();
        }

        [HttpPut("change-notifications-setting")]
        public async Task<IActionResult> ChangeNotificationsSettings(bool isEnabled)
        {
            await _internalValueService.UpdateValueAsync(InternalValues.IsProgressNotificationsEnabled, isEnabled).ConfigureAwait(false);

            return Ok();
        }

        [HttpPut("change-chunks-storage")]
        public async Task<IActionResult> ChangeChunksStorage(StorageSetting storageSetting)
        {
            await _internalValueService.UpdateValueAsync(InternalValues.ChunksStorageSetting, storageSetting).ConfigureAwait(false);

            return Ok();
        }

        [HttpPut("clean-up")]
        public IActionResult CleanUp([FromBody]CleanUpSettingsModel cleanUpSettingsModel)
        {
            var passwordHash = _authenticationService.GenerateHash(cleanUpSettingsModel.Password);
            if (passwordHash != _appSettings.SecurityPasswordHash)
                return BadRequest();

            var deleteBefore = DateTime.UtcNow.AddDays(-cleanUpSettingsModel.DeleteBeforeInDays);
            BackgroundJob.Enqueue(() => _cleanUpService.CleanUp(deleteBefore, cleanUpSettingsModel.CleanUpSettings, cleanUpSettingsModel.ForceCleanUp));

            return Ok();
        }

        [HttpDelete("clean-chunks")]
        public async Task<IActionResult> CleanOutdatedChunks()
        {
            await _uploadedChunkService.CleanOutdatedChunksAsync().ConfigureAwait(false);

            return Ok();
        }

        [HttpPut("subscription-recalculation")]
        public async Task<IActionResult> SubscriptionRecalculation()
        {
            await _userSubscriptionService.RecalculateCurrentUserSubscriptions().ConfigureAwait(false);

            return Ok();
        }
    }
}