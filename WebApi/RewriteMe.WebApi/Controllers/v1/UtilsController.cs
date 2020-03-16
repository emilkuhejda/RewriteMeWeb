using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RewriteMe.Domain.Enums;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.Domain.Settings;
using RewriteMe.WebApi.Extensions;
using RewriteMe.WebApi.Models;
using RewriteMe.WebApi.Utils;
using Serilog;
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
        private readonly AppSettings _appSettings;
        private readonly ILogger _logger;

        public UtilsController(
            IAuthenticationService authenticationService,
            IOptions<AppSettings> options,
            ILogger logger)
        {
            _authenticationService = authenticationService;
            _appSettings = options.Value;
            _logger = logger.ForContext<UtilsController>();
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
        public IActionResult RefreshToken()
        {
            var userId = HttpContext.User.GetNameIdentifier();
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Role, Role.User.ToString())
            };

            var token = TokenHelper.Generate(_appSettings.SecretKey, claims, TimeSpan.FromDays(180));

            _logger.Information($"Token was refreshed. [{userId}]");

            return Ok(token);
        }

        [AllowAnonymous]
        [HttpPost("generate-token")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> CreateToken([FromForm]CreateTokenModel createTokenModel)
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

            _logger.Information($"Token was created. [{createTokenModel.UserId}]");

            return Ok(token);
        }
    }
}