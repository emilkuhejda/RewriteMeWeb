using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RewriteMe.Domain.Dtos;
using RewriteMe.Domain.Enums;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.Domain.Recording;
using RewriteMe.WebApi.Commands;
using RewriteMe.WebApi.Extensions;
using RewriteMe.WebApi.Models;
using Serilog;
using Swashbuckle.AspNetCore.Annotations;

namespace RewriteMe.WebApi.Controllers.V1
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/speech-results")]
    [Produces("application/json")]
    [Authorize(Roles = nameof(Role.User))]
    [ApiController]
    public class SpeechResultsController : ControllerBase
    {
        private readonly ISpeechResultService _speechResultService;
        private readonly IMediator _mediator;
        private readonly ILogger _logger;

        public SpeechResultsController(
            ISpeechResultService speechResultService,
            IMediator mediator,
            ILogger logger)
        {
            _speechResultService = speechResultService;
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost("create")]
        [ProducesResponseType(typeof(OkDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(OperationId = "CreateSpeechResult")]
        public async Task<IActionResult> Create(CreateSpeechResultModel createSpeechResultModel)
        {
            var userId = HttpContext.User.GetNameIdentifier();
            var speechResult = createSpeechResultModel.ToSpeechResult();
            await _speechResultService.AddAsync(speechResult).ConfigureAwait(false);

            _logger.Information($"User with ID='{userId}' inserted speech result: {speechResult}. [{userId}]");

            return Ok(new OkDto());
        }

        [HttpPut("update")]
        [ProducesResponseType(typeof(TimeSpanWrapperDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(OperationId = "UpdateSpeechResults")]
        public async Task<IActionResult> Update(IEnumerable<SpeechResultModel> speechResultModels)
        {
            var userId = HttpContext.User.GetNameIdentifier();
            var speechResults = speechResultModels.Select(x => new SpeechResult { Id = x.Id, TotalTime = TimeSpan.FromTicks(x.Ticks) }).ToList();
            var updateSpeechResultsCommand = new UpdateSpeechResultsCommand
            {
                UserId = userId,
                SpeechResults = speechResults
            };
            var timeSpanWrapperDto = await _mediator.Send(updateSpeechResultsCommand).ConfigureAwait(false);

            return Ok(timeSpanWrapperDto);
        }
    }
}