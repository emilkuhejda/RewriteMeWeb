using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RewriteMe.Domain.Enums;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.Domain.Polling;
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

        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetRandom()
        {
            var rnd = new Random();
            var number = rnd.Next(0, 100);
            var fileItem = new Guid("51B8652A-7D5C-41F5-A203-E9CD9DCB1087");
            var cacheItem = new CacheItem(new Guid("07784962-6eed-4e4c-9b22-77113b921122"), fileItem, RecognitionState.Converting);
            _cacheService.AddItem(fileItem, cacheItem);

            return Ok();
        }
    }
}