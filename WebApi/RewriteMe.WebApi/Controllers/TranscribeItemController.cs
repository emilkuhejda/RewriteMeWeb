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

namespace RewriteMe.WebApi.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Authorize]
    [ApiController]
    public class TranscribeItemController : ControllerBase
    {
        private readonly ITranscribeItemService _transcribeItemService;

        public TranscribeItemController(ITranscribeItemService transcribeItemService)
        {
            _transcribeItemService = transcribeItemService;
        }

        [HttpGet("/api/transcribe-items-all")]
        [ProducesResponseType(typeof(IEnumerable<TranscribeItemDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(OperationId = "GetTranscribeItemsAll")]
        public async Task<ActionResult> GetAll(DateTimeOffset updatedAfter, Guid applicationId)
        {
            var userId = HttpContext.User.GetNameIdentifier();
            var transcribeItems = await _transcribeItemService.GetAllAsync(userId, updatedAfter.DateTime, applicationId).ConfigureAwait(false);

            return Ok(transcribeItems.Select(x => x.ToDto()));
        }

        [HttpGet("/api/transcribe-items/audio/{transcribeItemId}")]
        [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
        [SwaggerOperation(OperationId = "GetTranscribeAudioSource")]
        public async Task<ActionResult> GetAudioSource(Guid transcribeItemId)
        {
            var transcribeItem = await _transcribeItemService.GetAsync(transcribeItemId).ConfigureAwait(false);
            return new FileContentResult(transcribeItem.Source, "audio/wav");
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