using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RewriteMe.Domain.Enums;
using RewriteMe.Domain.Interfaces.Services;

namespace RewriteMe.WebApi.Controllers.ControlPanel
{
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

        [HttpGet("/api/control-panel/users")]
        public async Task<IActionResult> Get()
        {
            var users = await _userService.GetAllAsync().ConfigureAwait(false);

            return Ok(users);
        }

        [HttpDelete("/api/control-panel/users/delete")]
        public async Task<IActionResult> DeleteUser(Guid userId, string email)
        {
            var userExists = await _userService.ExistsAsync(userId, email).ConfigureAwait(false);
            if (!userExists)
                return BadRequest();

            var isSuccess = await _userService.DeleteAsync(userId).ConfigureAwait(false);
            if (!isSuccess)
                return NotFound();

            return Ok();
        }
    }
}