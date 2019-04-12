using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.WebApi.Extensions;
using RewriteMe.WebApi.Models;

namespace RewriteMe.WebApi.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Authorize]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAuthenticationService _authenticationService;

        public UserController(IUserService userService, IAuthenticationService authenticationService)
        {
            _userService = userService;
            _authenticationService = authenticationService;
        }

        [AllowAnonymous]
        [HttpPost("/api/users/register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Register([FromBody] RegisterUserModel registerUserModel)
        {
            var user = registerUserModel.ToUser();
            var userAlreadyExists = await _userService.UserAlreadyExistsAsync(user).ConfigureAwait(false);
            if (userAlreadyExists)
                return Conflict();

            _authenticationService.CalculatePasswordHash(user, registerUserModel.Password);
            await _userService.AddAsync(user).ConfigureAwait(false);

            return Ok();
        }
    }
}