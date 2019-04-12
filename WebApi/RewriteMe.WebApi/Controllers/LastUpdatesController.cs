using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.WebApi.Dtos;

namespace RewriteMe.WebApi.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Authorize]
    [ApiController]
    public class LastUpdatesController : ControllerBase
    {
        private readonly IFileItemService _fileItemService;
        private readonly IAudioSourceService _audioSourceService;
        private readonly ITranscribeItemService _transcribeItemService;

        public LastUpdatesController(
            IFileItemService fileItemService,
            IAudioSourceService audioSourceService,
            ITranscribeItemService transcribeItemService)
        {
            _fileItemService = fileItemService;
            _audioSourceService = audioSourceService;
            _transcribeItemService = transcribeItemService;
        }

        [HttpGet("/api/lastUpdates")]
        [ProducesResponseType(typeof(LastUpdatesDto), StatusCodes.Status200OK)]
        public async Task<ActionResult> Get()
        {
            var userId = Guid.Parse(HttpContext.User.Identity.Name);

            var fileItemLastVersion = await _fileItemService.GetLastVersion(userId).ConfigureAwait(false);
            var audioSourceLastVersion = await _audioSourceService.GetLastVersion(userId).ConfigureAwait(false);
            var transcribeItemLastVersion = await _transcribeItemService.GetLastVersion(userId).ConfigureAwait(false);

            return Ok(new LastUpdatesDto
            {
                FileItem = fileItemLastVersion,
                AudioSource = audioSourceLastVersion,
                TranscribeItem = transcribeItemLastVersion
            });
        }
    }
}