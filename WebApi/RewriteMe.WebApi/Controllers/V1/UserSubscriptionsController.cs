using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RewriteMe.Domain.Dtos;
using RewriteMe.Domain.Enums;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.Domain.Settings;
using RewriteMe.Domain.Transcription;
using RewriteMe.WebApi.Commands;
using RewriteMe.WebApi.Extensions;
using Swashbuckle.AspNetCore.Annotations;

namespace RewriteMe.WebApi.Controllers.V1
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/subscriptions")]
    [Produces("application/json")]
    [Authorize(Roles = nameof(Role.User))]
    [ApiController]
    public class UserSubscriptionsController : ControllerBase
    {
        private readonly IUserSubscriptionService _userSubscriptionService;
        private readonly IMediator _mediator;
        private readonly AppSettings _appSettings;

        public UserSubscriptionsController(
            IUserSubscriptionService userSubscriptionService,
            IMediator mediator,
            IOptions<AppSettings> options)
        {
            _userSubscriptionService = userSubscriptionService;
            _mediator = mediator;
            _appSettings = options.Value;
        }

        [HttpPost("create")]
        [ProducesResponseType(typeof(TimeSpanWrapperDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorCode), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(OperationId = "CreateUserSubscription")]
        public async Task<IActionResult> Create([FromBody]BillingPurchase billingPurchase, Guid applicationId)
        {
            var userId = HttpContext.User.GetNameIdentifier();
            var createUserSubscriptionCommand = new CreateUserSubscriptionCommand
            {
                UserId = userId,
                BillingPurchase = billingPurchase,
                ApplicationId = applicationId
            };

            var timeSpanWrapperDto = await _mediator.Send(createUserSubscriptionCommand).ConfigureAwait(false);
            return Ok(timeSpanWrapperDto);
        }

        [HttpGet("speech-configuration")]
        [ProducesResponseType(typeof(SpeechConfigurationDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(OperationId = "GetSpeechConfiguration")]
        public async Task<IActionResult> GetSpeechConfiguration()
        {
            var userId = HttpContext.User.GetNameIdentifier();
            var createSpeechConfigurationCommand = new CreateSpeechConfigurationCommand
            {
                UserId = userId,
                AppSettings = _appSettings
            };

            var speechConfigurationDto = await _mediator.Send(createSpeechConfigurationCommand).ConfigureAwait(false);
            return Ok(speechConfigurationDto);
        }

        [HttpGet("remaining-time")]
        [ProducesResponseType(typeof(TimeSpanWrapperDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(OperationId = "GetSubscriptionRemainingTime")]
        public async Task<IActionResult> GetSubscriptionRemainingTime()
        {
            var userId = HttpContext.User.GetNameIdentifier();
            var remainingTime = await _userSubscriptionService.GetRemainingTimeAsync(userId).ConfigureAwait(false);
            var timeSpanWrapperDto = new TimeSpanWrapperDto { Ticks = remainingTime.Ticks };

            return Ok(timeSpanWrapperDto);
        }
    }
}