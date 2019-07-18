using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.WebApi.Dtos;
using RewriteMe.WebApi.Extensions;
using RewriteMe.WebApi.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace RewriteMe.WebApi.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Authorize]
    [ApiController]
    public class SpeechResultController : Controller
    {
        private readonly IUserSubscriptionService _userSubscriptionService;
        private readonly ISpeechResultService _speechResultService;
        private readonly IApplicationLogService _applicationLogService;

        public SpeechResultController(
            IUserSubscriptionService userSubscriptionService,
            ISpeechResultService speechResultService,
            IApplicationLogService applicationLogService)
        {
            _userSubscriptionService = userSubscriptionService;
            _speechResultService = speechResultService;
            _applicationLogService = applicationLogService;
        }

        [HttpPut("/api/speech-results/create")]
        [ProducesResponseType(typeof(OkDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status426UpgradeRequired)]
        [SwaggerOperation(OperationId = "CreateSpeechResult")]
        public async Task<IActionResult> Create([FromForm] CreateSpeechResultModel createSpeechResultModel)
        {
            var userId = HttpContext.User.GetNameIdentifier();
            var subscriptionRemainingTime = await _userSubscriptionService.GetRemainingTime(userId).ConfigureAwait(false);
            var remainingTime = subscriptionRemainingTime.Subtract(createSpeechResultModel.TotalTime);

            var speechResult = createSpeechResultModel.ToSpeechResult();
            await _speechResultService.AddAsync(speechResult).ConfigureAwait(false);

            await _applicationLogService
                .InfoAsync($"User with ID='{userId}' inserted speech result: {speechResult}.", userId)
                .ConfigureAwait(false);

            if (remainingTime.Ticks < 0)
            {
                await _applicationLogService
                    .WarningAsync($"Speech result was successfully inserted. No additional free minutes for user with ID='{userId}'.", userId)
                    .ConfigureAwait(false);

                return StatusCode(426);
            }

            return Ok(new OkDto());
        }
    }
}