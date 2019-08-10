using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.Domain.Settings;
using RewriteMe.WebApi.Dtos;
using RewriteMe.WebApi.Extensions;
using RewriteMe.WebApi.Models;
using RewriteMe.WebApi.Utils;
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
        private readonly AppSettings _appSettings;

        public UserController(
            IUserService userService,
            IUserSubscriptionService userSubscriptionService,
            IApplicationLogService applicationLogService,
            IOptions<AppSettings> options)
        {
            _userService = userService;
            _userSubscriptionService = userSubscriptionService;
            _applicationLogService = applicationLogService;
            _appSettings = options.Value;
        }

        [HttpPost("/api/b2c/users/register")]
        [ProducesResponseType(typeof(RegistrationModelDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [SwaggerOperation(OperationId = "RegisterUser")]
        public async Task<IActionResult> Register([FromBody] RegisterUserModel registerUserModel)
        {
            await _applicationLogService.InfoAsync($"Attempt to register user with ID = '{registerUserModel.Id}'.").ConfigureAwait(false);

            UserSubscriptionDto userSubscriptionDto;
            var user = await _userService.GetAsync(registerUserModel.Id).ConfigureAwait(false);
            if (user == null)
            {
                user = registerUserModel.ToUser();
                await _userService.AddAsync(user).ConfigureAwait(false);
                await _applicationLogService.InfoAsync($"User with ID = '{user.Id}' and Email = '{user.Email}' was created.").ConfigureAwait(false);

                var userSubscription = new UserSubscription
                {
                    Id = Guid.NewGuid(),
                    UserId = user.Id,
                    ApplicationId = user.ApplicationId,
                    Time = TimeSpan.FromMinutes(5),
                    DateCreated = DateTime.UtcNow
                };

                await _userSubscriptionService.AddAsync(userSubscription).ConfigureAwait(false);
                await _applicationLogService.InfoAsync($"Basic 5 minutes subscription with ID = '{userSubscription.Id}' was created.", user.Id).ConfigureAwait(false);

                userSubscriptionDto = userSubscription.ToDto();
            }
            else
            {
                userSubscriptionDto = new UserSubscriptionDto { Id = Guid.Empty };
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var token = TokenHelper.Generate(_appSettings.SecretKey, claims, 180);

            var registrationModelDto = new RegistrationModelDto
            {
                Token = token,
                UserSubscription = userSubscriptionDto
            };

            return Ok(registrationModelDto);
        }
    }
}