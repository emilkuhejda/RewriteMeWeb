using System;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RewriteMe.Common.Utils;
using RewriteMe.Domain.Dtos;
using RewriteMe.Domain.Enums;
using RewriteMe.Domain.Extensions;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.Domain.Settings;
using RewriteMe.WebApi.Models;
using RewriteMe.WebApi.Utils;

namespace RewriteMe.WebApi.Controllers.V1
{
    [ApiVersion("1")]
    [Produces("application/json")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IApplicationLogService _applicationLogService;
        private readonly AppSettings _appSettings;

        public AuthenticationController(
            IAuthenticationService authenticationService,
            IApplicationLogService applicationLogService,
            IOptions<AppSettings> options)
        {
            _authenticationService = authenticationService;
            _applicationLogService = applicationLogService;
            _appSettings = options.Value;
        }

        [AllowAnonymous]
        [HttpPost("/api/v{version:apiVersion}/authenticate")]
        [ProducesResponseType(typeof(AdministratorDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Authenticate([FromBody]AuthenticationModel authenticationModel)
        {
            try
            {
                var administrator = await _authenticationService.AuthenticateAsync(authenticationModel.Username, authenticationModel.Password).ConfigureAwait(false);
                if (administrator == null)
                    return NotFound();

                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, administrator.Id.ToString()),
                    new Claim(ClaimTypes.Role, Role.Admin.ToString())
                };

                var token = TokenHelper.Generate(_appSettings.SecretKey, claims, TimeSpan.FromDays(7));

                return Ok(administrator.ToDto(token));
            }
            catch (Exception ex)
            {
                await _applicationLogService.ErrorAsync($"{ExceptionFormatter.FormatException(ex)}").ConfigureAwait(false);
            }

            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }
}