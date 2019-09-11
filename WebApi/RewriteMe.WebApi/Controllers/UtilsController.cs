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
using RewriteMe.WebApi.Utils;
using Swashbuckle.AspNetCore.Annotations;

namespace RewriteMe.WebApi.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Authorize]
    [ApiController]
    public class UtilsController : RewriteMeControllerBase
    {
        private readonly AppSettings _appSettings;

        public UtilsController(
            IOptions<AppSettings> options,
            IUserService userService)
            : base(userService)
        {
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
        [Authorize(Roles = nameof(Role.User))]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [SwaggerOperation(OperationId = "RefreshToken")]
        public async Task<IActionResult> RefreshToken()
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

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet("/api/generate-hangfire-access")]
        [Authorize(Roles = nameof(Role.Admin))]
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
    }
}