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
using RewriteMe.Domain.Models;
using RewriteMe.Domain.Settings;
using RewriteMe.WebApi.Extensions;
using RewriteMe.WebApi.Utils;
using Swashbuckle.AspNetCore.Annotations;

namespace RewriteMe.WebApi.Controllers.V1
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/utils")]
    [Produces("application/json")]
    [ApiController]
    public class UtilsController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IApplicationLogService _applicationLogService;
        private readonly AppSettings _appSettings;

        public UtilsController(
            IAuthenticationService authenticationService,
            IApplicationLogService applicationLogService,
            IOptions<AppSettings> options)
        {
            _authenticationService = authenticationService;
            _applicationLogService = applicationLogService;
            _appSettings = options.Value;
        }

        [AllowAnonymous]
        [HttpGet("is-alive")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [SwaggerOperation(OperationId = "IsAlive")]
        public IActionResult IsAlive()
        {
            return Ok(true);
        }

        [HttpGet("refresh-token")]
        [Authorize(Roles = nameof(Role.Security))]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(OperationId = "RefreshToken")]
        public async Task<IActionResult> RefreshToken()
        {
            try
            {
                var userId = HttpContext.User.GetNameIdentifier();
                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                    new Claim(ClaimTypes.Role, Role.User.ToString())
                };

                var token = TokenHelper.Generate(_appSettings.SecretKey, claims, TimeSpan.FromDays(180));
                return Ok(token);
            }
            catch (Exception ex)
            {
                await _applicationLogService.ErrorAsync($"{ExceptionFormatter.FormatException(ex)}").ConfigureAwait(false);
            }

            return StatusCode((int)HttpStatusCode.InternalServerError);
        }

        [AllowAnonymous]
        [HttpPost("generate-token")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> CreateToken([FromForm]CreateTokenModel createTokenModel)
        {
            try
            {
                var administrator = await _authenticationService.AuthenticateAsync(createTokenModel.Username, createTokenModel.Password).ConfigureAwait(false);
                if (administrator == null)
                    return NotFound();

                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, createTokenModel.UserId.ToString()),
                    new Claim(ClaimTypes.Role, createTokenModel.Role.ToString())
                };

                var token = TokenHelper.Generate(_appSettings.SecretKey, claims, TimeSpan.FromDays(180));
                return Ok(token);
            }
            catch (Exception ex)
            {
                await _applicationLogService.ErrorAsync($"{ExceptionFormatter.FormatException(ex)}").ConfigureAwait(false);
            }

            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }
}