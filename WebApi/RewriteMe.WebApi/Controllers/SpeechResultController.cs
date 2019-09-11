using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RewriteMe.Domain.Enums;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.Domain.Recording;
using RewriteMe.WebApi.Dtos;
using RewriteMe.WebApi.Extensions;
using RewriteMe.WebApi.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace RewriteMe.WebApi.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Authorize(Roles = nameof(Role.User))]
    [Authorize]
    [ApiController]
    public class SpeechResultController : RewriteMeControllerBase
    {
        private readonly IRecognizedAudioSampleService _recognizedAudioSampleService;
        private readonly ISpeechResultService _speechResultService;
        private readonly IApplicationLogService _applicationLogService;

        public SpeechResultController(
            IRecognizedAudioSampleService recognizedAudioSampleService,
            ISpeechResultService speechResultService,
            IApplicationLogService applicationLogService,
            IUserService userService)
            : base(userService)
        {
            _recognizedAudioSampleService = recognizedAudioSampleService;
            _speechResultService = speechResultService;
            _applicationLogService = applicationLogService;
        }

        [HttpPost("/api/speech-results/create")]
        [ProducesResponseType(typeof(OkDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [SwaggerOperation(OperationId = "CreateSpeechResult")]
        public async Task<IActionResult> Create([FromForm] CreateSpeechResultModel createSpeechResultModel)
        {
            var user = await VerifyUserAsync().ConfigureAwait(false);
            if (user == null)
                return StatusCode(401);

            var speechResult = createSpeechResultModel.ToSpeechResult();
            await _speechResultService.AddAsync(speechResult).ConfigureAwait(false);

            await _applicationLogService
                .InfoAsync($"User with ID='{user.Id}' inserted speech result: {speechResult}.", user.Id)
                .ConfigureAwait(false);

            return Ok(new OkDto());
        }

        [HttpPut("/api/speech-results/update")]
        [ProducesResponseType(typeof(OkDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [SwaggerOperation(OperationId = "UpdateSpeechResults")]
        public async Task<IActionResult> Update(IEnumerable<SpeechResultModel> speechResultModels)
        {
            var user = await VerifyUserAsync().ConfigureAwait(false);
            if (user == null)
                return StatusCode(401);

            var speechResults = speechResultModels.Select(x => new SpeechResult { Id = x.Id, TotalTime = x.TotalTime });
            await _speechResultService.UpdateAllAsync(speechResults).ConfigureAwait(false);

            await _applicationLogService.InfoAsync("Update speech results total time.", user.Id).ConfigureAwait(false);

            return Ok(new OkDto());
        }

        [HttpGet("/api/speech-results/recognized-time")]
        [ProducesResponseType(typeof(RecognizedTimeDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [SwaggerOperation(OperationId = "GetRecognizedTime")]
        public async Task<IActionResult> GetRecognizedTime()
        {
            var user = await VerifyUserAsync().ConfigureAwait(false);
            if (user == null)
                return StatusCode(401);

            var recognizedTime = await _recognizedAudioSampleService.GetRecognizedTime(user.Id).ConfigureAwait(false);
            var recognizedTimeDto = new RecognizedTimeDto
            {
                TotalTimeTicks = recognizedTime.Ticks
            };

            return Ok(recognizedTimeDto);
        }
    }
}