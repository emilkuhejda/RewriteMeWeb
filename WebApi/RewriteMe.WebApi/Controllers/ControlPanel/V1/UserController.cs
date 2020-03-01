using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RewriteMe.Domain.Enums;
using RewriteMe.Domain.Interfaces.Services;

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

        public UserController(IUserService userService)
        {
            _userService = userService;
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
            var userExists = await _userService.ExistsAsync(userId, email).ConfigureAwait(false);
            if (!userExists)
                return NotFound();

            var isSuccess = await _userService.DeleteAsync(userId).ConfigureAwait(false);
            if (!isSuccess)
                return BadRequest();

            return Ok();
        }
    }
}