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

        public SpeechResultController(
            IUserSubscriptionService userSubscriptionService,
            ISpeechResultService speechResultService)
        {
            _userSubscriptionService = userSubscriptionService;
            _speechResultService = speechResultService;
        }

        [HttpPut("/api/speech-results/create")]
        [ProducesResponseType(typeof(OkDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status426UpgradeRequired)]
        [SwaggerOperation(OperationId = "CreateSpeechResult")]
        public async Task<IActionResult> Create([FromForm] CreateSpeechResultModel createSpeechResultModel)
        {
            var userId = HttpContext.User.GetNameIdentifier();
            var speechResult = createSpeechResultModel.ToSpeechResult();
            await _speechResultService.AddAsync(speechResult).ConfigureAwait(false);

            var subscriptionRemainingTime = await _userSubscriptionService.GetRemainingTime(userId).ConfigureAwait(false);
            var remainingTime = subscriptionRemainingTime.Subtract(createSpeechResultModel.TotalTime);
            if (remainingTime.Ticks < 0)
                return StatusCode(426);

            return Ok(new OkDto());
        }
    }
}