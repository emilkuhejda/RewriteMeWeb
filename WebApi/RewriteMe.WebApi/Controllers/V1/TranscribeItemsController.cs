using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RewriteMe.Domain.Dtos;
using RewriteMe.Domain.Enums;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.WebApi.Extensions;
using RewriteMe.WebApi.Models;
using Serilog;
using Swashbuckle.AspNetCore.Annotations;

namespace RewriteMe.WebApi.Controllers.V1
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/transcribe-items")]
    [Produces("application/json")]
    [Authorize(Roles = nameof(Role.User))]
    [ApiController]
    public class TranscribeItemsController : ControllerBase
    {
        private readonly ITranscribeItemService _transcribeItemService;
        private readonly ILogger _logger;

        public TranscribeItemsController(
            ITranscribeItemService transcribeItemService,
            ILogger logger)
        {
            _transcribeItemService = transcribeItemService;
            _logger = logger.ForContext<TranscribeItemsController>();
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<TranscribeItemDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(OperationId = "GetTranscribeItemsAll")]
        public async Task<ActionResult> GetAll(DateTime updatedAfter, Guid applicationId)
        {
            _logger.Information($"Retrieve data for minimum date '{updatedAfter}'.");

            var userId = HttpContext.User.GetNameIdentifier();
            var transcribeItems = await _transcribeItemService.GetAllForUserAsync(userId, updatedAfter, applicationId).ConfigureAwait(false);

            return Ok(transcribeItems.Select(x => x.ToDto()));
        }

        [HttpGet("{fileItemId}")]
        [ProducesResponseType(typeof(IEnumerable<TranscribeItemDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(OperationId = "GetTranscribeItems")]
        public async Task<ActionResult> GetAllByFileItemId(Guid fileItemId)
        {
            var transcribeItems = await _transcribeItemService.GetAllAsync(fileItemId).ConfigureAwait(false);

            return Ok(transcribeItems.Select(x => x.ToDto()));
        }

        [HttpGet("audio/{transcribeItemId}")]
        [ProducesResponseType(typeof(byte[]), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(OperationId = "GetTranscribeAudioSource")]
        public async Task<ActionResult> GetAudioSource(Guid transcribeItemId)
        {
            var userId = HttpContext.User.GetNameIdentifier();
            var source = await _transcribeItemService.GetSourceAsync(userId, transcribeItemId).ConfigureAwait(false);
            if (source == null)
            {
                _logger.Error($"Transcribe item '{transcribeItemId}' was not found.");

                return NotFound();
            }

            return Ok(source);
        }

        [HttpGet("audio-stream/{transcribeItemId}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(OperationId = "GetTranscribeAudioSourceStream")]
        public async Task<ActionResult> GetAudioSourceStream(Guid transcribeItemId)
        {
            var userId = HttpContext.User.GetNameIdentifier();
            var source = await _transcribeItemService.GetSourceAsync(userId, transcribeItemId).ConfigureAwait(false);
            if (source == null)
            {
                _logger.Error($"Transcribe item '{transcribeItemId}' was not found.");

                return NotFound();
            }

            return new FileContentResult(source, "audio/wav");
        }

        [HttpPut("update-transcript")]
        [ProducesResponseType(typeof(OkDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(OperationId = "UpdateUserTranscript")]
        public async Task<ActionResult> UpdateUserTranscript(UpdateTranscribeItemModel updateTranscribeItemModel)
        {
            var dateUpdated = DateTime.UtcNow;
            await _transcribeItemService.UpdateUserTranscriptAsync(updateTranscribeItemModel.TranscribeItemId, updateTranscribeItemModel.Transcript, dateUpdated, updateTranscribeItemModel.ApplicationId).ConfigureAwait(false);

            _logger.Error($"User transcript was updated. Transcribe item model: {JsonConvert.SerializeObject(updateTranscribeItemModel)}");

            return Ok(new OkDto(dateUpdated));
        }
    }
}