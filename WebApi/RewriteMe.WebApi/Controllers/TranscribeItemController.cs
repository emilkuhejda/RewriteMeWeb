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

        [HttpGet("/api/transcribe-items/{fileItemId}")]
        [ProducesResponseType(typeof(IEnumerable<TranscribeItemDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult> GetAll(Guid fileItemId)
        {
            var transcribeItems = await _transcribeItemService.GetAll(fileItemId).ConfigureAwait(false);

            return Ok(transcribeItems.Select(x => x.ToDto()));
        }

        [HttpGet("/api/transcribe-items/audio/{transcribeItemId}")]
        [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
        public async Task<ActionResult> GetAudioSource(Guid transcribeItemId)
        {
            var transcribeItem = await _transcribeItemService.Get(transcribeItemId).ConfigureAwait(false);
            return new FileContentResult(transcribeItem.Source, "audio/wav");
        }

        [HttpPut("/api/transcribe-items/update-transcript")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> UpdateUserTranscript([FromForm] UpdateTranscribeItem updateTranscribeItem)
        {
            await _transcribeItemService
                .UpdateUserTranscript(updateTranscribeItem.TranscribeItemId, updateTranscribeItem.Transcript, updateTranscribeItem.Version)
                .ConfigureAwait(false);

            return Ok();
        }
    }
}