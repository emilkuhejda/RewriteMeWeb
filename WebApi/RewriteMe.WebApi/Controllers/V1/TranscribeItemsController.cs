using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RewriteMe.Common.Utils;
using RewriteMe.Domain.Dtos;
using RewriteMe.Domain.Enums;
using RewriteMe.Domain.Extensions;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.Domain.Models;
using RewriteMe.WebApi.Extensions;
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
        private readonly IApplicationLogService _applicationLogService;

        public TranscribeItemsController(
            ITranscribeItemService transcribeItemService,
            IApplicationLogService applicationLogService)
        {
            _transcribeItemService = transcribeItemService;
            _applicationLogService = applicationLogService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<TranscribeItemDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(OperationId = "GetTranscribeItemsAll")]
        public async Task<ActionResult> GetAll(DateTime updatedAfter, Guid applicationId)
        {
            try
            {
                var userId = HttpContext.User.GetNameIdentifier();
                var transcribeItems = await _transcribeItemService.GetAllForUserAsync(userId, updatedAfter.ToUniversalTime(), applicationId).ConfigureAwait(false);

                return Ok(transcribeItems.Select(x => x.ToDto()));
            }
            catch (Exception ex)
            {
                await _applicationLogService.ErrorAsync($"{ExceptionFormatter.FormatException(ex)}").ConfigureAwait(false);
            }

            return StatusCode((int)HttpStatusCode.InternalServerError);
        }

        [HttpGet("{fileItemId}")]
        [ProducesResponseType(typeof(IEnumerable<TranscribeItemDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(OperationId = "GetTranscribeItems")]
        public async Task<ActionResult> GetAllByFileItemId(Guid fileItemId)
        {
            try
            {
                var transcribeItems = await _transcribeItemService.GetAllAsync(fileItemId).ConfigureAwait(false);

                return Ok(transcribeItems.Select(x => x.ToDto()));
            }
            catch (Exception ex)
            {
                await _applicationLogService.ErrorAsync($"{ExceptionFormatter.FormatException(ex)}").ConfigureAwait(false);
            }

            return StatusCode((int)HttpStatusCode.InternalServerError);
        }

        [HttpGet("audio/{transcribeItemId}")]
        [ProducesResponseType(typeof(byte[]), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(OperationId = "GetTranscribeAudioSource")]
        public async Task<ActionResult> GetAudioSource(Guid transcribeItemId)
        {
            try
            {
                var userId = HttpContext.User.GetNameIdentifier();
                var source = await _transcribeItemService.GetSourceAsync(userId, transcribeItemId).ConfigureAwait(false);
                if (source == null)
                    return NotFound();

                return Ok(source);
            }
            catch (Exception ex)
            {
                await _applicationLogService.ErrorAsync($"{ExceptionFormatter.FormatException(ex)}").ConfigureAwait(false);
            }

            return StatusCode((int)HttpStatusCode.InternalServerError);
        }

        [HttpGet("audio-stream/{transcribeItemId}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(OperationId = "GetTranscribeAudioSourceStream")]
        public async Task<ActionResult> GetAudioSourceStream(Guid transcribeItemId)
        {
            try
            {
                var userId = HttpContext.User.GetNameIdentifier();
                var source = await _transcribeItemService.GetSourceAsync(userId, transcribeItemId).ConfigureAwait(false);
                if (source == null)
                    return NotFound();

                return new FileContentResult(source, "audio/wav");
            }
            catch (Exception ex)
            {
                await _applicationLogService.ErrorAsync($"{ExceptionFormatter.FormatException(ex)}").ConfigureAwait(false);
            }

            return StatusCode((int)HttpStatusCode.InternalServerError);
        }

        [HttpPut("update-transcript")]
        [ProducesResponseType(typeof(OkDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(OperationId = "UpdateUserTranscript")]
        public async Task<ActionResult> UpdateUserTranscript(UpdateTranscribeItemModel updateTranscribeItemModel)
        {
            try
            {
                var dateUpdated = DateTime.UtcNow;
                await _transcribeItemService.UpdateUserTranscriptAsync(updateTranscribeItemModel.TranscribeItemId, updateTranscribeItemModel.Transcript, dateUpdated, updateTranscribeItemModel.ApplicationId).ConfigureAwait(false);

                return Ok(new OkDto(dateUpdated));
            }
            catch (Exception ex)
            {
                await _applicationLogService.ErrorAsync($"{ExceptionFormatter.FormatException(ex)}").ConfigureAwait(false);
            }

            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }
}