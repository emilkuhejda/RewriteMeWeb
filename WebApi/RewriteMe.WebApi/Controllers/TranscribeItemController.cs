using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.WebApi.Dtos;
using RewriteMe.WebApi.Extensions;
using RewriteMe.WebApi.Models;
using Swashbuckle.AspNetCore.Annotations;
using IOFile = System.IO.File;

namespace RewriteMe.WebApi.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Authorize]
    [ApiController]
    public class TranscribeItemController : ControllerBase
    {
        private readonly ITranscribeItemService _transcribeItemService;
        private readonly IFileAccessService _fileAccessService;

        public TranscribeItemController(
            ITranscribeItemService transcribeItemService,
            IFileAccessService fileAccessService)
        {
            _transcribeItemService = transcribeItemService;
            _fileAccessService = fileAccessService;
        }

        [HttpGet("/api/transcribe-items/{fileItemId}")]
        [ProducesResponseType(typeof(IEnumerable<TranscribeItemDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(OperationId = "GetTranscribeItems")]
        public async Task<ActionResult> GetAll(Guid fileItemId)
        {
            var transcribeItems = await _transcribeItemService.GetAllAsync(fileItemId).ConfigureAwait(false);

            return Ok(transcribeItems.Select(x => x.ToDto()));
        }

        [HttpGet("/api/transcribe-items-all")]
        [ProducesResponseType(typeof(IEnumerable<TranscribeItemDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(OperationId = "GetTranscribeItemsAll")]
        public async Task<ActionResult> GetAll(DateTime updatedAfter, Guid applicationId)
        {
            var userId = HttpContext.User.GetNameIdentifier();
            var transcribeItems = await _transcribeItemService.GetAllForUserAsync(userId, updatedAfter.ToUniversalTime(), applicationId).ConfigureAwait(false);

            return Ok(transcribeItems.Select(x => x.ToDto()));
        }

        [HttpGet("/api/transcribe-items/audio/{transcribeItemId}")]
        [ProducesResponseType(typeof(byte[]), StatusCodes.Status200OK)]
        [SwaggerOperation(OperationId = "GetTranscribeAudioSource")]
        public async Task<ActionResult> GetAudioSource(Guid transcribeItemId)
        {
            var transcribeItem = await _transcribeItemService.GetAsync(transcribeItemId).ConfigureAwait(false);
            var sourcePath = _fileAccessService.GetTranscriptionPath(transcribeItem);
            var source = await IOFile.ReadAllBytesAsync(sourcePath).ConfigureAwait(false);
            return Ok(source);
        }

        [HttpGet("/api/transcribe-items/audio-stream/{transcribeItemId}")]
        [SwaggerOperation(OperationId = "GetTranscribeAudioSourceStream")]
        public async Task<ActionResult> GetAudioSourceStream(Guid transcribeItemId)
        {
            var transcribeItem = await _transcribeItemService.GetAsync(transcribeItemId).ConfigureAwait(false);
            var sourcePath = _fileAccessService.GetTranscriptionPath(transcribeItem);
            var source = await IOFile.ReadAllBytesAsync(sourcePath).ConfigureAwait(false);
            return new FileContentResult(source, "audio/wav");
        }

        [HttpPut("/api/transcribe-items/update-transcript")]
        [ProducesResponseType(typeof(OkDto), StatusCodes.Status200OK)]
        [SwaggerOperation(OperationId = "UpdateUserTranscript")]
        public async Task<ActionResult> UpdateUserTranscript([FromForm] UpdateTranscribeItem updateTranscribeItem)
        {
            var dateUpdated = DateTime.UtcNow;
            await _transcribeItemService
                .UpdateUserTranscriptAsync(updateTranscribeItem.TranscribeItemId, updateTranscribeItem.Transcript, dateUpdated, updateTranscribeItem.ApplicationId)
                .ConfigureAwait(false);

            return Ok(new OkDto(dateUpdated));
        }
    }
}