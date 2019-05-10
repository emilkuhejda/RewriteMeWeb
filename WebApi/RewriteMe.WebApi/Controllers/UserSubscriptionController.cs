using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.Domain.Transcription;
using RewriteMe.WebApi.Dtos;
using RewriteMe.WebApi.Extensions;
using Swashbuckle.AspNetCore.Annotations;

namespace RewriteMe.WebApi.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Authorize]
    [ApiController]
    public class UserSubscriptionController : ControllerBase
    {
        private readonly IUserSubscriptionService _userSubscriptionService;

        public UserSubscriptionController(IUserSubscriptionService userSubscriptionService)
        {
            _userSubscriptionService = userSubscriptionService;
        }

        [HttpGet("/api/subscriptions")]
        [ProducesResponseType(typeof(IEnumerable<UserSubscriptionDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(OperationId = "GetUserSubscriptions")]
        public async Task<IActionResult> GetAll(DateTimeOffset updatedAfter, Guid applicationId)
        {
            var userId = HttpContext.User.GetNameIdentifier();
            var userSubscriptions = await _userSubscriptionService.GetAllAsync(userId, updatedAfter.DateTime, applicationId).ConfigureAwait(false);

            return Ok(userSubscriptions.Select(x => x.ToDto()));
        }

        [HttpGet("/api/subscriptions/products")]
        [ProducesResponseType(typeof(IEnumerable<SubscriptionProductDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(OperationId = "GetAvailableProducts")]
        public IActionResult GetAvailableProducts()
        {
            return Ok(SubscriptionProducts.All.Select(x => x.ToDto()));
        }

        [HttpPost("/api/subscriptions/create")]
        [ProducesResponseType(typeof(UserSubscriptionDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [SwaggerOperation(OperationId = "CreateUserSubscription")]
        public async Task<IActionResult> Create([FromBody]BillingPurchase billingPurchase, Guid applicationId)
        {
            var userId = HttpContext.User.GetNameIdentifier();
            if (userId != billingPurchase.UserId)
                return StatusCode(409);

            var userSubscription = await _userSubscriptionService.RegisterPurchaseAsync(billingPurchase, applicationId).ConfigureAwait(false);
            if (userSubscription == null)
                return StatusCode(406);

            return Ok(userSubscription);
        }
    }
}