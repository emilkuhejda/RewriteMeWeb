using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.Domain.Settings;
using RewriteMe.WebApi.Dtos;
using RewriteMe.WebApi.Extensions;
using RewriteMe.WebApi.Models;

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

        [HttpPost("/authenticate")]
        [ProducesResponseType(typeof(AdministratorDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticationModel authenticationModel)
        {
            var administrator = await _authenticationService.AuthenticateAsync(authenticationModel.Username, authenticationModel.Password).ConfigureAwait(false);
            if (administrator == null)
                return Forbid();

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.SecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, administrator.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(administrator.ToDto(tokenString));
        }
    }
}