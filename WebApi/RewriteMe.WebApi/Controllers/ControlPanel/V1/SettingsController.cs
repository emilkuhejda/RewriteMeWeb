using System;
using System.Net;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RewriteMe.Business.Configuration;
using RewriteMe.Common.Utils;
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
        private readonly IApplicationLogService _applicationLogService;
        private readonly AppSettings _appSettings;

        public SettingsController(
            IInternalValueService internalValueService,
            ICleanUpService cleanUpService,
            IAuthenticationService authenticationService,
            IApplicationLogService applicationLogService,
            IOptions<AppSettings> options)
        {
            _internalValueService = internalValueService;
            _cleanUpService = cleanUpService;
            _authenticationService = authenticationService;
            _applicationLogService = applicationLogService;
            _appSettings = options.Value;
        }

        [HttpGet("storage-setting")]
        public async Task<IActionResult> GetStorageSetting()
        {
            try
            {
                var storageSetting = await _internalValueService.GetValueAsync(InternalValues.StorageSetting).ConfigureAwait(false);

                return Ok(storageSetting);
            }
            catch (Exception ex)
            {
                await _applicationLogService.ErrorAsync($"{ExceptionFormatter.FormatException(ex)}").ConfigureAwait(false);
            }

            return StatusCode((int)HttpStatusCode.InternalServerError);
        }

        [HttpGet("database-backup")]
        public async Task<IActionResult> GetDatabaseBackupSetting()
        {
            try
            {
                var isDatabaseBackupEnabled = await _internalValueService.GetValueAsync(InternalValues.IsDatabaseBackupEnabled).ConfigureAwait(false);

                return Ok(isDatabaseBackupEnabled);
            }
            catch (Exception ex)
            {
                await _applicationLogService.ErrorAsync($"{ExceptionFormatter.FormatException(ex)}").ConfigureAwait(false);
            }

            return StatusCode((int)HttpStatusCode.InternalServerError);
        }

        [HttpGet("notifications-setting")]
        public async Task<IActionResult> GetNotificationsSetting()
        {
            try
            {
                var isProgressNotificationsEnabled = await _internalValueService.GetValueAsync(InternalValues.IsProgressNotificationsEnabled).ConfigureAwait(false);

                return Ok(isProgressNotificationsEnabled);
            }
            catch (Exception ex)
            {
                await _applicationLogService.ErrorAsync($"{ExceptionFormatter.FormatException(ex)}").ConfigureAwait(false);
            }

            return StatusCode((int)HttpStatusCode.InternalServerError);
        }

        [HttpPut("change-storage")]
        public async Task<IActionResult> ChangeStorage(StorageSetting storageSetting)
        {
            try
            {
                await _internalValueService.UpdateValueAsync(InternalValues.StorageSetting, storageSetting).ConfigureAwait(false);

                return Ok();
            }
            catch (Exception ex)
            {
                await _applicationLogService.ErrorAsync($"{ExceptionFormatter.FormatException(ex)}").ConfigureAwait(false);
            }

            return StatusCode((int)HttpStatusCode.InternalServerError);
        }

        [HttpPut("change-database-backup")]
        public async Task<IActionResult> ChangeDatabaseBackupSettings(bool isEnabled)
        {
            try
            {
                await _internalValueService.UpdateValueAsync(InternalValues.IsDatabaseBackupEnabled, isEnabled).ConfigureAwait(false);

                return Ok();
            }
            catch (Exception ex)
            {
                await _applicationLogService.ErrorAsync($"{ExceptionFormatter.FormatException(ex)}").ConfigureAwait(false);
            }

            return StatusCode((int)HttpStatusCode.InternalServerError);
        }

        [HttpPut("change-notifications-setting")]
        public async Task<IActionResult> ChangeNotificationsSettings(bool isEnabled)
        {
            try
            {
                await _internalValueService.UpdateValueAsync(InternalValues.IsProgressNotificationsEnabled, isEnabled).ConfigureAwait(false);

                return Ok();
            }
            catch (Exception ex)
            {
                await _applicationLogService.ErrorAsync($"{ExceptionFormatter.FormatException(ex)}").ConfigureAwait(false);
            }

            return StatusCode((int)HttpStatusCode.InternalServerError);
        }

        [HttpPut("clean-up")]
        public async Task<IActionResult> CleanUp([FromBody]CleanUpSettingsModel cleanUpSettingsModel)
        {
            try
            {
                var passwordHash = _authenticationService.GenerateHash(cleanUpSettingsModel.Password);
                if (passwordHash != _appSettings.SecurityPasswordHash)
                    return BadRequest();

                var deleteBefore = DateTime.UtcNow.AddDays(-cleanUpSettingsModel.DeleteBeforeInDays);
                BackgroundJob.Enqueue(() => _cleanUpService.CleanUp(deleteBefore, cleanUpSettingsModel.CleanUpSettings, cleanUpSettingsModel.ForceCleanUp));

                return Ok();
            }
            catch (Exception ex)
            {
                await _applicationLogService.ErrorAsync($"{ExceptionFormatter.FormatException(ex)}").ConfigureAwait(false);
            }

            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }
}