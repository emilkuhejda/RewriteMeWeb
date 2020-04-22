using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RewriteMe.Domain.Interfaces.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace RewriteMe.WebApi.Controllers.V1
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/recognition-cache")]
    [Produces("application/json")]
    [ApiController]
    public class SpeechRecognitionCacheController : ControllerBase
    {
        private readonly ISpeechRecognitionCacheService _speechRecognitionCacheService;

        public SpeechRecognitionCacheController(ISpeechRecognitionCacheService speechRecognitionCacheService)
        {
            _speechRecognitionCacheService = speechRecognitionCacheService;
        }

        [HttpGet("{fileItemId}")]
        [ProducesResponseType(typeof(double), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(OperationId = "GetPercentage")]
        public IActionResult GetPercentage(Guid fileItemId)
        {
            var percentage = _speechRecognitionCacheService.GetPercentage(fileItemId);

            return Ok(percentage);
        }
    }
}