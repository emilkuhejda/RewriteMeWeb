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
using RewriteMe.WebApi.Dtos;
using RewriteMe.WebApi.Extensions;
using RewriteMe.WebApi.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace RewriteMe.WebApi.Controllers.V1
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/transcribe-items")]
    [Produces("application/json")]
    [Authorize(Roles = nameof(Role.User))]
    [ApiController]
    public class TranscribeItemsController : RewriteMeControllerBase
    {
        private readonly ITranscribeItemService _transcribeItemService;
        private readonly IApplicationLogService _applicationLogService;

        public TranscribeItemsController(
            ITranscribeItemService transcribeItemService,
            IApplicationLogService applicationLogService,
            IUserService userService)
            : base(userService)
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
                var user = await VerifyUserAsync().ConfigureAwait(false);
                if (user == null)
                    return StatusCode(401);

                var transcribeItems = await _transcribeItemService.GetAllForUserAsync(user.Id, updatedAfter.ToUniversalTime(), applicationId).ConfigureAwait(false);

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
                var user = await VerifyUserAsync().ConfigureAwait(false);
                if (user == null)
                    return StatusCode(401);

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
                var user = await VerifyUserAsync().ConfigureAwait(false);
                if (user == null)
                    return StatusCode(401);

                var source = await _transcribeItemService.GetSourceAsync(transcribeItemId).ConfigureAwait(false);
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
                var user = await VerifyUserAsync().ConfigureAwait(false);
                if (user == null)
                    return StatusCode(401);

                var source = await _transcribeItemService.GetSourceAsync(transcribeItemId).ConfigureAwait(false);
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
        public async Task<ActionResult> UpdateUserTranscript([FromForm] UpdateTranscribeItem updateTranscribeItem)
        {
            try
            {
                var user = await VerifyUserAsync().ConfigureAwait(false);
                if (user == null)
                    return StatusCode(401);

                var dateUpdated = DateTime.UtcNow;
                await _transcribeItemService.UpdateUserTranscriptAsync(updateTranscribeItem.TranscribeItemId, updateTranscribeItem.Transcript, dateUpdated, updateTranscribeItem.ApplicationId).ConfigureAwait(false);

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