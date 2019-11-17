using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RewriteMe.Common.Utils;
using RewriteMe.Domain.Enums;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.Domain.Recording;
using RewriteMe.Domain.Settings;
using RewriteMe.Domain.Transcription;
using RewriteMe.WebApi.Dtos;
using Swashbuckle.AspNetCore.Annotations;

namespace RewriteMe.WebApi.Controllers.V1
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/subscriptions")]
    [Produces("application/json")]
    [Authorize(Roles = nameof(Role.User))]
    [ApiController]
    public class UserSubscriptionsController : RewriteMeControllerBase
    {
        private readonly IUserSubscriptionService _userSubscriptionService;
        private readonly IRecognizedAudioSampleService _recognizedAudioSampleService;
        private readonly IApplicationLogService _applicationLogService;
        private readonly AppSettings _appSettings;

        public UserSubscriptionsController(
            IUserSubscriptionService userSubscriptionService,
            IRecognizedAudioSampleService recognizedAudioSampleService,
            IApplicationLogService applicationLogService,
            IOptions<AppSettings> options,
            IUserService userService)
            : base(userService)
        {
            _userSubscriptionService = userSubscriptionService;
            _recognizedAudioSampleService = recognizedAudioSampleService;
            _applicationLogService = applicationLogService;
            _appSettings = options.Value;
        }

        [HttpPost("create")]
        [ProducesResponseType(typeof(TimeSpanWrapperDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(OperationId = "CreateUserSubscription")]
        public async Task<IActionResult> Create([FromBody]BillingPurchase billingPurchase, Guid applicationId)
        {
            try
            {
                var user = await VerifyUserAsync().ConfigureAwait(false);
                if (user == null)
                    return StatusCode(401);

                if (user.Id != billingPurchase.UserId)
                    return StatusCode(409);

                var userSubscription = await _userSubscriptionService.RegisterPurchaseAsync(billingPurchase, applicationId).ConfigureAwait(false);
                if (userSubscription == null)
                    return StatusCode(406);

                var remainingTime = await _userSubscriptionService.GetRemainingTimeAsync(user.Id).ConfigureAwait(false);
                var timeSpanWrapperDto = new TimeSpanWrapperDto { Ticks = remainingTime.Ticks };

                return Ok(timeSpanWrapperDto);
            }
            catch (Exception ex)
            {
                await _applicationLogService.ErrorAsync($"{ExceptionFormatter.FormatException(ex)}").ConfigureAwait(false);
            }

            return StatusCode((int)HttpStatusCode.InternalServerError);
        }

        [HttpGet("speech-configuration")]
        [ProducesResponseType(typeof(SpeechConfigurationDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(OperationId = "GetSpeechConfiguration")]
        public async Task<IActionResult> GetSpeechConfiguration()
        {
            try
            {
                var user = await VerifyUserAsync().ConfigureAwait(false);
                if (user == null)
                    return StatusCode(401);

                var recognizedAudioSample = new RecognizedAudioSample
                {
                    Id = Guid.NewGuid(),
                    UserId = user.Id,
                    DateCreatedUtc = DateTime.UtcNow
                };

                await _recognizedAudioSampleService.AddAsync(recognizedAudioSample).ConfigureAwait(false);

                var remainingTime = await _userSubscriptionService.GetRemainingTimeAsync(user.Id).ConfigureAwait(false);
                var speechConfigurationDto = new SpeechConfigurationDto
                {
                    SubscriptionKey = _appSettings.AzureSubscriptionKey,
                    SpeechRegion = _appSettings.AzureSpeechRegion,
                    AudioSampleId = recognizedAudioSample.Id,
                    SubscriptionRemainingTimeTicks = remainingTime.Ticks
                };

                await _applicationLogService.InfoAsync($"User with ID='{user.Id}' retrieved speech recognition configuration: {speechConfigurationDto}.", user.Id).ConfigureAwait(false);

                return Ok(speechConfigurationDto);
            }
            catch (Exception ex)
            {
                await _applicationLogService.ErrorAsync($"{ExceptionFormatter.FormatException(ex)}").ConfigureAwait(false);
            }

            return StatusCode((int)HttpStatusCode.InternalServerError);
        }

        [HttpGet("remaining-time")]
        [ProducesResponseType(typeof(TimeSpanWrapperDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(OperationId = "GetSubscriptionRemainingTime")]
        public async Task<IActionResult> GetSubscriptionRemainingTime()
        {
            try
            {
                var user = await VerifyUserAsync().ConfigureAwait(false);
                if (user == null)
                    return StatusCode(401);

                var remainingTime = await _userSubscriptionService.GetRemainingTimeAsync(user.Id).ConfigureAwait(false);
                var timeSpanWrapperDto = new TimeSpanWrapperDto { Ticks = remainingTime.Ticks };

                return Ok(timeSpanWrapperDto);
            }
            catch (Exception ex)
            {
                await _applicationLogService.ErrorAsync($"{ExceptionFormatter.FormatException(ex)}").ConfigureAwait(false);
            }

            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }
}