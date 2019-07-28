using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RewriteMe.Domain.Interfaces.Services;

namespace RewriteMe.WebApi.Controllers.ControlPanel
{
    [Route("control-panel/[controller]")]
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

        [HttpGet("/control-panel/users")]
        public async Task<IActionResult> Get()
        {
            var users = await _userService.GetAllAsync().ConfigureAwait(false);

            return Ok(users);
        }

        [HttpGet("/control-panel/purchases")]
        public async Task<IActionResult> GetPurchases()
        {
            return Ok();
        }

        [HttpGet("/control-panel/subscriptions")]
        public async Task<IActionResult> GetSubscriptions()
        {
            var userId = new Guid(HttpContext.User.Identity.Name);
            var billingPurchases = await _billingPurchaseService.GetAllByUserAsync(userId).ConfigureAwait(false);

            return Ok(billingPurchases);
        }
    }
}