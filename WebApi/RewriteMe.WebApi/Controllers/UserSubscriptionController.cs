using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RewriteMe.Domain.Interfaces.Services;
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
        public async Task<IActionResult> GetAll(DateTimeOffset updatedAfter)
        {
            var userId = HttpContext.User.GetNameIdentifier();
            var userSubscriptions = await _userSubscriptionService.GetAllAsync(userId, updatedAfter.DateTime).ConfigureAwait(false);

            return Ok(userSubscriptions.Select(x => x.ToDto()));
        }
    }
}