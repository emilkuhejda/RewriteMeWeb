using System;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RewriteMe.Common.Utils;
using RewriteMe.Domain.Enums;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.Domain.Settings;
using RewriteMe.WebApi.Models;
using RewriteMe.WebApi.Utils;

namespace RewriteMe.WebApi.Controllers.ControlPanel
{
    [Produces("application/json")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize(Roles = nameof(Role.Admin))]
    [ApiController]
    public class UtilsController : ControllerBase
    {
        private readonly ISpeechRecognitionService _speechRecognitionService;
        private readonly IDatabaseService _databaseService;
        private readonly IAuthenticationService _authenticationService;
        private readonly IApplicationLogService _applicationLogService;
        private readonly AppSettings _appSettings;

        public UtilsController(
            ISpeechRecognitionService speechRecognitionService,
            IDatabaseService databaseService,
            IAuthenticationService authenticationService,
            IApplicationLogService applicationLogService,
            IOptions<AppSettings> options)
        {
            _speechRecognitionService = speechRecognitionService;
            _databaseService = databaseService;
            _authenticationService = authenticationService;
            _applicationLogService = applicationLogService;
            _appSettings = options.Value;
        }

        [HttpGet("/api/control-panel/has-access")]
        public IActionResult HasAccess()
        {
            return Ok(true);
        }

        [HttpGet("/api/control-panel/is-deployment-successful")]
        public async Task<IActionResult> IsDeploymentSuccessful()
        {
            try
            {
                var canCreateSpeechClient = await _speechRecognitionService.CanCreateSpeechClientAsync().ConfigureAwait(false);
                if (!canCreateSpeechClient)
                    return BadRequest();

                return Ok();
            }
            catch (Exception ex)
            {
                await _applicationLogService.ErrorAsync($"{ExceptionFormatter.FormatException(ex)}").ConfigureAwait(false);
            }

            return StatusCode((int)HttpStatusCode.InternalServerError);
        }

        [HttpGet("/api/control-panel/generate-hangfire-access")]
        public async Task<IActionResult> GenerateHangfireAccess()
        {
            try
            {
                var claims = new[]
                {
                    new Claim(ClaimTypes.Role, Role.Admin.ToString())
                };

                var expireTime = TimeSpan.FromMinutes(5);
                var token = TokenHelper.Generate(_appSettings.HangfireSecretKey, claims, expireTime);
                var cookieOptions = new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.Add(expireTime),
                    Path = "/",
                    HttpOnly = false,
                    IsEssential = true
                };

                Response.Cookies.Append(Constants.HangfireAccessToken, token, cookieOptions);

                return Ok();
            }
            catch (Exception ex)
            {
                await _applicationLogService.ErrorAsync($"{ExceptionFormatter.FormatException(ex)}").ConfigureAwait(false);
            }

            return StatusCode((int)HttpStatusCode.InternalServerError);
        }

        [HttpPut("/api/control-panel/reset-database")]
        public async Task<IActionResult> ResetDatabase([FromBody]ResetDatabaseModel resetDatabaseModel)
        {
            try
            {
                var passwordHash = _authenticationService.GenerateHash(resetDatabaseModel.Password);
                if (passwordHash != _appSettings.SecurityPasswordHash)
                    return BadRequest();

                await _databaseService.ResetAsync().ConfigureAwait(false);

                return Ok();
            }
            catch (Exception ex)
            {
                await _applicationLogService.ErrorAsync($"{ExceptionFormatter.FormatException(ex)}").ConfigureAwait(false);
            }

            return StatusCode((int)HttpStatusCode.InternalServerError);
        }

        [HttpPut("/api/control-panel/delete-database")]
        public async Task<IActionResult> DeleteDatabase([FromBody]ResetDatabaseModel resetDatabaseModel)
        {
            try
            {
                var passwordHash = _authenticationService.GenerateHash(resetDatabaseModel.Password);
                if (passwordHash != _appSettings.SecurityPasswordHash)
                    return BadRequest();

                await _databaseService.DeleteDatabaseAsync().ConfigureAwait(false);

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