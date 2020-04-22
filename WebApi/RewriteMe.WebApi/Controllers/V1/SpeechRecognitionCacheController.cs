using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RewriteMe.Domain.Interfaces.Services;

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
        [AllowAnonymous]
        public IActionResult GetPercentage(Guid fileItemId)
        {
            var percentage = _speechRecognitionCacheService.GetPercentage(fileItemId);

            return Ok(percentage);
        }
    }
}