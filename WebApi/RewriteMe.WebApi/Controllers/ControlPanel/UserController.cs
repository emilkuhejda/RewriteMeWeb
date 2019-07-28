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
        private readonly IUserSubscriptionService _userSubscriptionService;

        public UserController(
            IUserService userService,
            IBillingPurchaseService billingPurchaseService,
            IUserSubscriptionService userSubscriptionService)
        {
            _userService = userService;
            _billingPurchaseService = billingPurchaseService;
            _userSubscriptionService = userSubscriptionService;
        }

        [HttpGet("/control-panel/users")]
        public async Task<IActionResult> Get()
        {
            var users = await _userService.GetAllAsync().ConfigureAwait(false);

            return Ok(users);
        }

        [HttpGet("/control-panel/purchases/{userId}")]
        public async Task<IActionResult> GetPurchases(Guid userId)
        {
            var billingPurchases = await _billingPurchaseService.GetAllByUserAsync(userId).ConfigureAwait(false);

            return Ok(billingPurchases);
        }

        [HttpGet("/control-panel/subscriptions/{userId}")]
        public async Task<IActionResult> GetSubscriptions(Guid userId)
        {
            var userSubscriptions = await _userSubscriptionService.GetAllAsync(userId).ConfigureAwait(false);

            return Ok(userSubscriptions);
        }
    }
}