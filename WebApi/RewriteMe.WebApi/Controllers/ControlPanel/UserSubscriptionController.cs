using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RewriteMe.Common.Utils;
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
        private readonly IApplicationLogService _applicationLogService;

        public UserSubscriptionController(
            IUserSubscriptionService userSubscriptionService,
            IApplicationLogService applicationLogService)
        {
            _userSubscriptionService = userSubscriptionService;
            _applicationLogService = applicationLogService;
        }

        [HttpPost("/api/control-panel/subscriptions/create")]
        public async Task<IActionResult> Create([FromForm]CreateSubscriptionModel createSubscriptionModel)
        {
            try
            {
                var userSubscription = new UserSubscription
                {
                    Id = Guid.NewGuid(),
                    UserId = createSubscriptionModel.UserId,
                    ApplicationId = createSubscriptionModel.ApplicationId,
                    Time = TimeSpan.FromSeconds(createSubscriptionModel.Seconds),
                    DateCreatedUtc = DateTime.UtcNow
                };

                await _userSubscriptionService.AddAsync(userSubscription).ConfigureAwait(false);

                return Ok();
            }
            catch (Exception ex)
            {
                await _applicationLogService.ErrorAsync($"{ExceptionFormatter.FormatException(ex)}").ConfigureAwait(false);
            }

            return StatusCode((int)HttpStatusCode.InternalServerError);
        }

        [HttpGet("/api/control-panel/subscriptions/{userId}")]
        public async Task<IActionResult> GetSubscriptions(Guid userId)
        {
            try
            {
                var userSubscriptions = await _userSubscriptionService.GetAllAsync(userId).ConfigureAwait(false);

                return Ok(userSubscriptions);
            }
            catch (Exception ex)
            {
                await _applicationLogService.ErrorAsync($"{ExceptionFormatter.FormatException(ex)}").ConfigureAwait(false);
            }

            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }
}