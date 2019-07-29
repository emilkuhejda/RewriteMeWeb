﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.Domain.Settings;
using RewriteMe.WebApi.Models;

namespace RewriteMe.WebApi.Controllers.ControlPanel
{
    [Route("control-panel/[controller]")]
    [Produces("application/json")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize]
    [ApiController]
    public class UserSubscriptionController : ControllerBase
    {
        private readonly IUserSubscriptionService _userSubscriptionService;

        public UserSubscriptionController(IUserSubscriptionService userSubscriptionService)
        {
            _userSubscriptionService = userSubscriptionService;
        }

        [HttpPost("/control-panel/subscriptions/create")]
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

        [HttpGet("/control-panel/subscriptions/{userId}")]
        public async Task<IActionResult> GetSubscriptions(Guid userId)
        {
            var userSubscriptions = await _userSubscriptionService.GetAllAsync(userId).ConfigureAwait(false);

            return Ok(userSubscriptions);
        }
    }
}