using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RewriteMe.Common.Utils;
using RewriteMe.Domain.Enums;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.Domain.Recording;
using RewriteMe.WebApi.Dtos;
using RewriteMe.WebApi.Extensions;
using RewriteMe.WebApi.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace RewriteMe.WebApi.Controllers.V1
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/speech-results")]
    [Produces("application/json")]
    [Authorize(Roles = nameof(Role.User))]
    [ApiController]
    public class SpeechResultsController : RewriteMeControllerBase
    {
        private readonly ISpeechResultService _speechResultService;
        private readonly IUserSubscriptionService _userSubscriptionService;
        private readonly IApplicationLogService _applicationLogService;

        public SpeechResultsController(
            ISpeechResultService speechResultService,
            IUserSubscriptionService userSubscriptionService,
            IApplicationLogService applicationLogService,
            IUserService userService)
            : base(userService)
        {
            _speechResultService = speechResultService;
            _userSubscriptionService = userSubscriptionService;
            _applicationLogService = applicationLogService;
        }

        [HttpPost("create")]
        [ProducesResponseType(typeof(OkDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(OperationId = "CreateSpeechResult")]
        public async Task<IActionResult> Create(CreateSpeechResultModel createSpeechResultModel)
        {
            try
            {
                var user = await VerifyUserAsync().ConfigureAwait(false);
                if (user == null)
                    return StatusCode(401);

                var speechResult = createSpeechResultModel.ToSpeechResult();
                await _speechResultService.AddAsync(speechResult).ConfigureAwait(false);

                await _applicationLogService.InfoAsync($"User with ID='{user.Id}' inserted speech result: {speechResult}.", user.Id).ConfigureAwait(false);

                return Ok(new OkDto());
            }
            catch (Exception ex)
            {
                await _applicationLogService.ErrorAsync($"{ExceptionFormatter.FormatException(ex)}").ConfigureAwait(false);
            }

            return StatusCode((int)HttpStatusCode.InternalServerError);
        }

        [HttpPut("update")]
        [ProducesResponseType(typeof(TimeSpanWrapperDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(OperationId = "UpdateSpeechResults")]
        public async Task<IActionResult> Update(IEnumerable<SpeechResultModel> speechResultModels)
        {
            try
            {
                var user = await VerifyUserAsync().ConfigureAwait(false);
                if (user == null)
                    return StatusCode(401);

                var speechResults = speechResultModels.Select(x => new SpeechResult { Id = x.Id, TotalTime = x.TotalTime }).ToList();
                await _speechResultService.UpdateAllAsync(speechResults).ConfigureAwait(false);

                var totalTimeTicks = speechResults.Sum(x => x.TotalTime.Ticks);
                var totalTime = TimeSpan.FromTicks(totalTimeTicks);
                await _userSubscriptionService.SubtractTimeAsync(user.Id, totalTime).ConfigureAwait(false);

                await _applicationLogService.InfoAsync("Update speech results total time.", user.Id).ConfigureAwait(false);

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