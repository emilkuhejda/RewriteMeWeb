using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RewriteMe.Domain.Enums;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.Domain.Settings;
using RewriteMe.WebApi.Dtos;
using RewriteMe.WebApi.Extensions;
using RewriteMe.WebApi.Models;
using RewriteMe.WebApi.Utils;

namespace RewriteMe.WebApi.Controllers
{
    [Produces("application/json")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly AppSettings _appSettings;

        public AuthenticationController(
            IAuthenticationService authenticationService,
            IOptions<AppSettings> options)
        {
            _authenticationService = authenticationService;
            _appSettings = options.Value;
        }

        [AllowAnonymous]
        [HttpPost("/api/authenticate")]
        [ProducesResponseType(typeof(AdministratorDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticationModel authenticationModel)
        {
            var administrator = await _authenticationService.AuthenticateAsync(authenticationModel.Username, authenticationModel.Password).ConfigureAwait(false);
            if (administrator == null)
                return Forbid();

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, administrator.Id.ToString()),
                new Claim(ClaimTypes.Role, Role.Admin.ToString())
            };

            var token = TokenHelper.Generate(_appSettings.SecretKey, claims, 7);

            return Ok(administrator.ToDto(token));
        }
    }
}