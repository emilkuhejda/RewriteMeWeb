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
using Swashbuckle.AspNetCore.Annotations;

namespace RewriteMe.WebApi.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Authorize(Roles = nameof(Role.User))]
    [ApiController]
    public class UtilsController : RewriteMeControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IApplicationLogService _applicationLogService;
        private readonly AppSettings _appSettings;

        public UtilsController(
            IAuthenticationService authenticationService,
            IApplicationLogService applicationLogService,
            IOptions<AppSettings> options,
            IUserService userService)
            : base(userService)
        {
            _authenticationService = authenticationService;
            _applicationLogService = applicationLogService;
            _appSettings = options.Value;
        }

        [AllowAnonymous]
        [HttpGet("/api/is-alive")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [SwaggerOperation(OperationId = "IsAlive")]
        public IActionResult IsAlive()
        {
            return Ok(true);
        }

        [HttpGet("/api/refresh-token")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(OperationId = "RefreshToken")]
        public async Task<IActionResult> RefreshToken()
        {
            try
            {
                var user = await VerifyUserAsync().ConfigureAwait(false);
                if (user == null)
                    return StatusCode(401);

                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
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
        [HttpPost("/api/generate-token")]
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