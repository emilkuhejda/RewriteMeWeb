using Microsoft.AspNetCore.Authorization;
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
        public IActionResult Register([FromBody] RegisterUserModel registerUserModel)
        {
            var user = registerUserModel.ToUser();
            if (_userService.UserAlreadyExists(user))
                return Conflict();

            _authenticationService.CalculatePasswordHash(user, registerUserModel.Password);
            _userService.Add(user);

            return Ok();
        }
    }
}