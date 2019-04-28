using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.Domain.Settings;
using RewriteMe.WebApi.Dtos;
using RewriteMe.WebApi.Extensions;
using RewriteMe.WebApi.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace RewriteMe.WebApi.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Authorize]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IUserSubscriptionService _userSubscriptionService;
        private readonly IApplicationLogService _applicationLogService;

        public UserController(
            IUserService userService,
            IUserSubscriptionService userSubscriptionService,
            IApplicationLogService applicationLogService)
        {
            _userService = userService;
            _userSubscriptionService = userSubscriptionService;
            _applicationLogService = applicationLogService;
        }

        [AllowAnonymous]
        [HttpPost("/api/users/register")]
        [ProducesResponseType(typeof(OkDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [SwaggerOperation(OperationId = "RegisterUser")]
        public async Task<IActionResult> Register([FromBody] RegisterUserModel registerUserModel)
        {
            var user = registerUserModel.ToUser();
            var userAlreadyExists = await _userService.UserAlreadyExistsAsync(user.Id).ConfigureAwait(false);
            if (userAlreadyExists)
                return Conflict();

            await _userService.AddAsync(user).ConfigureAwait(false);
            await _applicationLogService.InfoAsync($"User with ID = '{user.Id}' and Email = '{user.Email}' was created.").ConfigureAwait(false);

            var userSubscription = new UserSubscription
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                Time = TimeSpan.FromMinutes(5),
                DateCreated = DateTime.UtcNow
            };

            await _userSubscriptionService.AddAsync(userSubscription).ConfigureAwait(false);
            await _applicationLogService.InfoAsync($"Basic 5 minutes subscription with ID = '{userSubscription.Id}' was created.", user.Id).ConfigureAwait(false);

            return Ok(new OkDto());
        }
    }
}