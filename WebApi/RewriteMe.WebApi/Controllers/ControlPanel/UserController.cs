using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RewriteMe.Domain.Interfaces.Services;

namespace RewriteMe.WebApi.Controllers.ControlPanel
{
    [Produces("application/json")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IBillingPurchaseService _billingPurchaseService;

        public UserController(
            IUserService userService,
            IBillingPurchaseService billingPurchaseService)
        {
            _userService = userService;
            _billingPurchaseService = billingPurchaseService;
        }

        [HttpGet("/api/control-panel/users")]
        public async Task<IActionResult> Get()
        {
            var users = await _userService.GetAllAsync().ConfigureAwait(false);

            return Ok(users);
        }

        [HttpGet("/api/control-panel/purchases/{userId}")]
        public async Task<IActionResult> GetPurchases(Guid userId)
        {
            var billingPurchases = await _billingPurchaseService.GetAllByUserAsync(userId).ConfigureAwait(false);

            return Ok(billingPurchases);
        }
    }
}