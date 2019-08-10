using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RewriteMe.Domain.Enums;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.Domain.Settings;
using RewriteMe.WebApi.Models;

namespace RewriteMe.WebApi.Controllers.ControlPanel
{
    [Produces("application/json")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize(Roles = nameof(Role.Admin))]
    [ApiController]
    public class UserSubscriptionController : ControllerBase
    {
        private readonly IUserSubscriptionService _userSubscriptionService;

        public UserSubscriptionController(IUserSubscriptionService userSubscriptionService)
        {
            _userSubscriptionService = userSubscriptionService;
        }

        [HttpPost("/api/control-panel/subscriptions/create")]
        public async Task<IActionResult> Create([FromForm]CreateSubscriptionModel createSubscriptionModel)
        {
            var userSubscription = new UserSubscription
            {
                Id = Guid.NewGuid(),
                UserId = createSubscriptionModel.UserId,
                ApplicationId = createSubscriptionModel.ApplicationId,
                Time = TimeSpan.FromMinutes(createSubscriptionModel.Minutes),
                DateCreated = DateTime.UtcNow
            };
            await _userSubscriptionService.AddAsync(userSubscription).ConfigureAwait(false);

            return Ok();
        }

        [HttpGet("/api/control-panel/subscriptions/{userId}")]
        public async Task<IActionResult> GetSubscriptions(Guid userId)
        {
            var userSubscriptions = await _userSubscriptionService.GetAllAsync(userId).ConfigureAwait(false);

            return Ok(userSubscriptions);
        }
    }
}