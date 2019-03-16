using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RewriteMe.Domain.Interfaces.Services;

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

            return Ok(transcribeItems);
        }

        [HttpGet("/api/transcribe-items/audio/{transcribeItemId}")]
        public async Task<ActionResult> GetAudioSource(Guid transcribeItemId)
        {
            var transcribeItem = await _transcribeItemService.Get(transcribeItemId).ConfigureAwait(false);
            return new FileContentResult(transcribeItem.Source, "audio/wav");
        }
    }
}