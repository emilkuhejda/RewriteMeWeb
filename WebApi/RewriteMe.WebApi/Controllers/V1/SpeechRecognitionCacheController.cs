using System;
using Microsoft.AspNetCore.Authorization;
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
        private readonly ICacheService _cacheService;

        public SpeechRecognitionCacheController(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }

        [HttpGet("{fileItemId}")]
        [ProducesResponseType(typeof(double), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(OperationId = "GetPercentage")]
        public IActionResult GetPercentage(Guid fileItemId)
        {
            var percentage = _cacheService.GetPercentage(fileItemId);

            return Ok(percentage);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetRandom()
        {
            var rnd = new Random();
            var number = rnd.Next(0, 100);
            _cacheService.AddOrUpdateItem(Guid.NewGuid(), number);

            return Ok();
        }
    }
}