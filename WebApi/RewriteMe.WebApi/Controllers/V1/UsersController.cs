using System;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RewriteMe.Business.Utils;
using RewriteMe.Common.Utils;
using RewriteMe.Domain.Enums;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.Domain.Settings;
using RewriteMe.WebApi.Dtos;
using RewriteMe.WebApi.Extensions;
using RewriteMe.WebApi.Models;
using RewriteMe.WebApi.Utils;
using Swashbuckle.AspNetCore.Annotations;

namespace RewriteMe.WebApi.Controllers.V1
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/users")]
    [Produces("application/json")]
    [Authorize]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IUserSubscriptionService _userSubscriptionService;
        private readonly IUserDeviceService _userDeviceService;
        private readonly IApplicationLogService _applicationLogService;
        private readonly AppSettings _appSettings;

        public UsersController(
            IUserService userService,
            IUserSubscriptionService userSubscriptionService,
            IUserDeviceService userDeviceService,
            IApplicationLogService applicationLogService,
            IOptions<AppSettings> options)
        {
            _userService = userService;
            _userSubscriptionService = userSubscriptionService;
            _userDeviceService = userDeviceService;
            _applicationLogService = applicationLogService;
            _appSettings = options.Value;
        }

        [HttpPut("update")]
        [Authorize(Roles = nameof(Role.User))]
        [ProducesResponseType(typeof(IdentityDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(OperationId = "UpdateUser")]
        public async Task<IActionResult> UpdateUser([FromBody]UpdateUserModel updateUserModel)
        {
            try
            {
                var userId = HttpContext.User.GetNameIdentifier();
                var user = await _userService.GetAsync(userId).ConfigureAwait(false);
                if (user == null)
                    return StatusCode(401);

                user.GivenName = updateUserModel.GivenName;
                user.FamilyName = updateUserModel.FamilyName;
                await _userService.UpdateAsync(user).ConfigureAwait(false);

                return Ok(user.ToIdentityDto());
            }
            catch (Exception ex)
            {
                await _applicationLogService.ErrorAsync($"{ExceptionFormatter.FormatException(ex)}").ConfigureAwait(false);
            }

            return StatusCode((int)HttpStatusCode.InternalServerError);
        }

        [HttpPost("/api/b2c/v{version:apiVersion}/users/register")]
        [ProducesResponseType(typeof(UserRegistrationDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(OperationId = "RegisterUser")]
        public async Task<IActionResult> Register([FromBody]RegistrationUserModel registrationUserModel)
        {
            try
            {
                await _applicationLogService.InfoAsync($"Attempt to register user with ID = '{registrationUserModel.Id}'.").ConfigureAwait(false);

                TimeSpan remainingTime;
                var user = await _userService.GetAsync(registrationUserModel.Id).ConfigureAwait(false);
                if (user == null)
                {
                    user = registrationUserModel.ToUser();
                    await _userService.AddAsync(user).ConfigureAwait(false);
                    await _applicationLogService.InfoAsync($"User with ID = '{user.Id}' and Email = '{user.Email}' was created.").ConfigureAwait(false);

                    var userSubscription = SubscriptionHelper.CreateFreeSubscription(user.Id, registrationUserModel.ApplicationId);
                    await _userSubscriptionService.AddAsync(userSubscription).ConfigureAwait(false);
                    await _applicationLogService.InfoAsync($"Basic {userSubscription.Time.TotalMinutes} minutes subscription with ID = '{userSubscription.Id}' was created.", user.Id).ConfigureAwait(false);

                    remainingTime = userSubscription.Time;
                }
                else
                {
                    remainingTime = await _userSubscriptionService.GetRemainingTimeAsync(user.Id).ConfigureAwait(false);
                }

                if (registrationUserModel.Device != null)
                {
                    var userDevice = registrationUserModel.Device.ToUserDevice(user.Id);
                    await _userDeviceService.AddOrUpdateAsync(userDevice).ConfigureAwait(false);
                }

                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, Role.User.ToString())
                };

                var token = TokenHelper.Generate(_appSettings.SecretKey, claims, TimeSpan.FromDays(180));

                var registrationModelDto = new UserRegistrationDto
                {
                    Token = token,
                    Identity = user.ToIdentityDto(),
                    RemainingTime = new TimeSpanWrapperDto { Ticks = remainingTime.Ticks }
                };

                return Ok(registrationModelDto);
            }
            catch (Exception ex)
            {
                await _applicationLogService.ErrorAsync($"{ExceptionFormatter.FormatException(ex)}").ConfigureAwait(false);
            }

            return StatusCode((int)HttpStatusCode.InternalServerError);
        }

        [HttpPut("update-language")]
        [Authorize(Roles = nameof(Role.User))]
        [ProducesResponseType(typeof(OkDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorCode), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(OperationId = "UpdateLanguage")]
        public async Task<IActionResult> UpdateLanguage(Guid installationId, int language)
        {
            try
            {
                var userId = HttpContext.User.GetNameIdentifier();
                if (!Enum.IsDefined(typeof(Language), language))
                    return BadRequest(ErrorCode.EC200);

                await _userDeviceService.UpdateLanguageAsync(userId, installationId, (Language)language).ConfigureAwait(false);

                return Ok(new OkDto());
            }
            catch (Exception ex)
            {
                await _applicationLogService.ErrorAsync($"{ExceptionFormatter.FormatException(ex)}").ConfigureAwait(false);
            }

            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }
}