using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.Domain.Recording;
using RewriteMe.Domain.Settings;
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
        private readonly IRecognizedAudioSampleService _recognizedAudioSampleService;
        private readonly IApplicationLogService _applicationLogService;
        private readonly AppSettings _appSettings;

        public UserSubscriptionController(
            IUserSubscriptionService userSubscriptionService,
            IRecognizedAudioSampleService recognizedAudioSampleService,
            IApplicationLogService applicationLogService,
            IOptions<AppSettings> options)
        {
            _userSubscriptionService = userSubscriptionService;
            _recognizedAudioSampleService = recognizedAudioSampleService;
            _applicationLogService = applicationLogService;
            _appSettings = options.Value;
        }

        [HttpGet("/api/subscriptions")]
        [ProducesResponseType(typeof(IEnumerable<UserSubscriptionDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(OperationId = "GetUserSubscriptions")]
        public async Task<IActionResult> GetAll(DateTime updatedAfter, Guid applicationId)
        {
            var userId = HttpContext.User.GetNameIdentifier();
            var userSubscriptions = await _userSubscriptionService.GetAllAsync(userId, updatedAfter.ToUniversalTime(), applicationId).ConfigureAwait(false);

            return Ok(userSubscriptions.Select(x => x.ToDto()));
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

            return Ok(userSubscription.ToDto());
        }

        [HttpGet("/api/subscriptions/speech-configuration")]
        [ProducesResponseType(typeof(SpeechConfigurationDto), StatusCodes.Status200OK)]
        [SwaggerOperation(OperationId = "GetSpeechConfiguration")]
        public async Task<IActionResult> GetSpeechConfiguration()
        {
            var userId = HttpContext.User.GetNameIdentifier();
            var recognizedAudioSample = new RecognizedAudioSample
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                DateCreated = DateTime.UtcNow
            };

            await _recognizedAudioSampleService.AddAsync(recognizedAudioSample).ConfigureAwait(false);

            var remainingTime = await _userSubscriptionService.GetRemainingTime(userId).ConfigureAwait(false);
            var speechConfigurationDto = new SpeechConfigurationDto
            {
                SubscriptionKey = _appSettings.AzureSubscriptionKey,
                SpeechRegion = _appSettings.AzureSpeechRegion,
                AudioSampleId = recognizedAudioSample.Id,
                SubscriptionRemainingTimeTicks = remainingTime.Ticks
            };

            await _applicationLogService
                .InfoAsync($"User with ID='{userId}' retrieved speech recognition configuration: {speechConfigurationDto}.", userId)
                .ConfigureAwait(false);

            return Ok(speechConfigurationDto);
        }
    }
}