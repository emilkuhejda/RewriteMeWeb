using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RewriteMe.Common.Utils;
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
        private readonly IApplicationLogService _applicationLogService;

        public UserController(
            IUserService userService,
            IApplicationLogService applicationLogService)
        {
            _userService = userService;
            _applicationLogService = applicationLogService;
        }

        [HttpGet("/api/control-panel/users")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var users = await _userService.GetAllAsync().ConfigureAwait(false);

                return Ok(users);
            }
            catch (Exception ex)
            {
                await _applicationLogService.ErrorAsync($"{ExceptionFormatter.FormatException(ex)}").ConfigureAwait(false);
            }

            return StatusCode((int)HttpStatusCode.InternalServerError);
        }

        [HttpDelete("/api/control-panel/users/delete")]
        public async Task<IActionResult> DeleteUser(Guid userId, string email)
        {
            try
            {
                var userExists = await _userService.ExistsAsync(userId, email).ConfigureAwait(false);
                if (!userExists)
                    return BadRequest();

                var isSuccess = await _userService.DeleteAsync(userId).ConfigureAwait(false);
                if (!isSuccess)
                    return NotFound();

                return Ok();
            }
            catch (Exception ex)
            {
                await _applicationLogService.ErrorAsync($"{ExceptionFormatter.FormatException(ex)}").ConfigureAwait(false);
            }

            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }
}