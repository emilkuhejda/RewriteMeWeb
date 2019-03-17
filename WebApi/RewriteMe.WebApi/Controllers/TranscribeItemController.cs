using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RewriteMe.Domain.Interfaces.Services;
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
        public async Task<ActionResult> GetAll(Guid fileItemId)
        {
            var transcribeItems = await _transcribeItemService.GetAll(fileItemId).ConfigureAwait(false);

            return Ok(transcribeItems.Select(x => new
            {
                x.Id,
                x.Alternatives,
                x.UserTranscript,
                x.TotalTime,
                x.DateCreated
            }));
        }

        [HttpGet("/api/transcribe-items/audio/{transcribeItemId}")]
        public async Task<ActionResult> GetAudioSource(Guid transcribeItemId)
        {
            var transcribeItem = await _transcribeItemService.Get(transcribeItemId).ConfigureAwait(false);
            return new FileContentResult(transcribeItem.Source, "audio/wav");
        }

        [HttpPut("/api/transcribe-items/update-transcript")]
        public async Task<ActionResult> UpdateUserTranscript([FromForm] UpdateTranscribeItem updateTranscribeItem)
        {
            await _transcribeItemService
                .UpdateUserTranscript(updateTranscribeItem.TranscribeItemId, updateTranscribeItem.Transcript)
                .ConfigureAwait(false);

            return Ok();
        }
    }
}