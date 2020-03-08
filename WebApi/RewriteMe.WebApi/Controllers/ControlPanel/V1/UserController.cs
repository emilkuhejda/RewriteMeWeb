using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RewriteMe.Domain.Enums;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.WebApi.Commands;

namespace RewriteMe.WebApi.Controllers.ControlPanel.V1
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/control-panel/users")]
    [Produces("application/json")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize(Roles = nameof(Role.Admin))]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMediator _mediator;

        public UserController(
            IUserService userService,
            IMediator mediator)
        {
            _userService = userService;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var users = await _userService.GetAllAsync().ConfigureAwait(false);

            return Ok(users);
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteUser(Guid userId, string email)
        {
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