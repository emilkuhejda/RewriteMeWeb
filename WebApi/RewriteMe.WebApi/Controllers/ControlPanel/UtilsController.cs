using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RewriteMe.Domain.Enums;
using RewriteMe.Domain.Interfaces.Services;

namespace RewriteMe.WebApi.Controllers.ControlPanel
{
    [Produces("application/json")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize(Roles = nameof(Role.Admin))]
    [ApiController]
    public class UtilsController : ControllerBase
    {
        private readonly ISpeechRecognitionService _speechRecognitionService;

        public UtilsController(ISpeechRecognitionService speechRecognitionService)
        {
            _speechRecognitionService = speechRecognitionService;
        }

        [HttpGet("/api/control-panel/has-access")]
        public IActionResult HasAccess()
        {
            return Ok(true);
        }

        [HttpGet("/api/control-panel/is-deployment-successful")]
        public async Task<IActionResult> IsDeploymentSuccessful()
        {
            var canCreateSpeechClient = await _speechRecognitionService.CanCreateSpeechClientAsync().ConfigureAwait(false);
            if (!canCreateSpeechClient)
                return BadRequest();

            return Ok();
        }
    }
}