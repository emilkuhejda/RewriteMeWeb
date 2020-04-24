using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RewriteMe.Domain.Enums;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.Domain.Polling;
using RewriteMe.Domain.Settings;
using RewriteMe.WebApi.Models;
using RewriteMe.WebApi.Utils;

namespace RewriteMe.WebApi.Controllers.ControlPanel.V1
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/control-panel/utils")]
    [Produces("application/json")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize(Roles = nameof(Role.Admin))]
    [ApiController]
    public class UtilsController : ControllerBase
    {
        private readonly ISpeechRecognitionService _speechRecognitionService;
        private readonly IDatabaseService _databaseService;
        private readonly IAuthenticationService _authenticationService;
        private readonly ICacheService _cacheService;
        private readonly AppSettings _appSettings;

        public UtilsController(
            ISpeechRecognitionService speechRecognitionService,
            IDatabaseService databaseService,
            IAuthenticationService authenticationService,
            ICacheService cacheService,
            IOptions<AppSettings> options)
        {
            _speechRecognitionService = speechRecognitionService;
            _databaseService = databaseService;
            _authenticationService = authenticationService;
            _cacheService = cacheService;
            _appSettings = options.Value;
        }

        [HttpPut("send-message")]
        [ApiExplorerSettings(IgnoreApi = false)]
        public async Task<IActionResult> SendMessage(Guid userId, Guid fileItemId, double percentage)
        {
            var cacheItem = new CacheItem(userId, fileItemId, RecognitionState.None);
            await _cacheService.AddItemAsync(cacheItem).ConfigureAwait(false);
            await _cacheService.UpdateRecognitionStateAsync(fileItemId, RecognitionState.Converting).ConfigureAwait(false);
            await _cacheService.UpdateRecognitionStateAsync(fileItemId, RecognitionState.InProgress).ConfigureAwait(false);
            await _cacheService.UpdatePercentageAsync(fileItemId, percentage).ConfigureAwait(false);
            _cacheService.RemoveItem(fileItemId);

            return Ok();
        }

        [HttpGet("has-access")]
        public IActionResult HasAccess()
        {
            return Ok(true);
        }

        [HttpGet("is-deployment-successful")]
        public IActionResult IsDeploymentSuccessful()
        {
            var canCreateSpeechClient = _speechRecognitionService.CanCreateSpeechClientAsync();
            if (!canCreateSpeechClient)
                return BadRequest();

            return Ok();
        }

        [HttpGet("generate-hangfire-access")]
        public IActionResult GenerateHangfireAccess()
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

        [HttpPut("reset-database")]
        public async Task<IActionResult> ResetDatabase([FromBody]ResetDatabaseModel resetDatabaseModel)
        {
            var passwordHash = _authenticationService.GenerateHash(resetDatabaseModel.Password);
            if (passwordHash != _appSettings.SecurityPasswordHash)
                return BadRequest();

            await _databaseService.ResetAsync().ConfigureAwait(false);

            return Ok();
        }

        [HttpPut("delete-database")]
        public async Task<IActionResult> DeleteDatabase([FromBody]ResetDatabaseModel resetDatabaseModel)
        {
            var passwordHash = _authenticationService.GenerateHash(resetDatabaseModel.Password);
            if (passwordHash != _appSettings.SecurityPasswordHash)
                return BadRequest();

            await _databaseService.DeleteDatabaseAsync().ConfigureAwait(false);

            return Ok();
        }
    }
}