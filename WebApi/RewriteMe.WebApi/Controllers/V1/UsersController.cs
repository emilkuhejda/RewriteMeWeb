using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RewriteMe.Domain.Dtos;
using RewriteMe.Domain.Enums;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.Domain.Settings;
using RewriteMe.WebApi.Commands;
using RewriteMe.WebApi.Extensions;
using RewriteMe.WebApi.Models;
using Serilog;
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
        private readonly IUserDeviceService _userDeviceService;
        private readonly IMediator _mediator;
        private readonly AppSettings _appSettings;
        private readonly ILogger _logger;

        public UsersController(
            IUserService userService,
            IUserDeviceService userDeviceService,
            IMediator mediator,
            IOptions<AppSettings> options,
            ILogger logger)
        {
            _userService = userService;
            _userDeviceService = userDeviceService;
            _mediator = mediator;
            _appSettings = options.Value;
            _logger = logger.ForContext<UsersController>();
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
            var userId = HttpContext.User.GetNameIdentifier();
            var user = await _userService.GetAsync(userId).ConfigureAwait(false);
            if (user == null)
            {
                _logger.Error($"User '{userId}' was not found.");

                return StatusCode(401);
            }

            user.GivenName = updateUserModel.GivenName;
            user.FamilyName = updateUserModel.FamilyName;
            await _userService.UpdateAsync(user).ConfigureAwait(false);

            _logger.Information($"User '{userId}' was successfully updated.");

            return Ok(user.ToIdentityDto());
        }

        [HttpPost("/api/b2c/v{version:apiVersion}/users/register")]
        [ProducesResponseType(typeof(UserRegistrationDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(OperationId = "RegisterUser")]
        public async Task<IActionResult> Register([FromBody]RegistrationUserModel registrationUserModel)
        {
            var registerUserCommand = new RegisterUserCommand
            {
                RegistrationUserModel = registrationUserModel,
                AppSettings = _appSettings
            };

            var userRegistrationDto = await _mediator.Send(registerUserCommand).ConfigureAwait(false);

            _logger.Information($"User was successfully registered. User registration model: {JsonConvert.SerializeObject(registrationUserModel)}");

            return Ok(userRegistrationDto);
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
            var userId = HttpContext.User.GetNameIdentifier();
            if (!Enum.IsDefined(typeof(Language), language))
            {
                _logger.Error($"Language '{language}' is not supported. [{userId}]");

                return BadRequest(ErrorCode.EC200);
            }

            await _userDeviceService.UpdateLanguageAsync(userId, installationId, (Language)language).ConfigureAwait(false);

            _logger.Information($"Language '{language}' was updated for device. [{userId}]");

            return Ok(new OkDto());
        }

        [HttpDelete]
        [Authorize(Roles = nameof(Role.User))]
        [ProducesResponseType(typeof(OkDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(OperationId = "DeleteUser")]
        public async Task<IActionResult> DeleteUser(string email)
        {
            var userId = HttpContext.User.GetNameIdentifier();
            var deleteUserAccountCommand = new DeleteUserAccountCommand
            {
                UserId = userId,
                Email = email
            };
            await _mediator.Send(deleteUserAccountCommand).ConfigureAwait(false);

            return Ok();
        }
    }
}