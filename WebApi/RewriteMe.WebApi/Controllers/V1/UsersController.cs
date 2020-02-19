﻿using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RewriteMe.Domain.Dtos;
using RewriteMe.Domain.Enums;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.Domain.Settings;
using RewriteMe.WebApi.Commands;
using RewriteMe.WebApi.Extensions;
using RewriteMe.WebApi.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace RewriteMe.WebApi.Controllers.v1
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

        public UsersController(
            IUserService userService,
            IUserDeviceService userDeviceService,
            IMediator mediator,
            IOptions<AppSettings> options)
        {
            _userService = userService;
            _userDeviceService = userDeviceService;
            _mediator = mediator;
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
            var userId = HttpContext.User.GetNameIdentifier();
            var user = await _userService.GetAsync(userId).ConfigureAwait(false);
            if (user == null)
                return StatusCode(401);

            user.GivenName = updateUserModel.GivenName;
            user.FamilyName = updateUserModel.FamilyName;
            await _userService.UpdateAsync(user).ConfigureAwait(false);

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
                return BadRequest(ErrorCode.EC200);

            await _userDeviceService.UpdateLanguageAsync(userId, installationId, (Language)language).ConfigureAwait(false);

            return Ok(new OkDto());
        }
    }
}