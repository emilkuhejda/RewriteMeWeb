using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.WebApi.Dtos;
using RewriteMe.WebApi.Extensions;
using Swashbuckle.AspNetCore.Annotations;

namespace RewriteMe.WebApi.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Authorize]
    [ApiController]
    public class LastUpdatesController : ControllerBase
    {
        private readonly IFileItemService _fileItemService;
        private readonly ITranscribeItemService _transcribeItemService;
        private readonly IUserSubscriptionService _userSubscriptionService;

        public LastUpdatesController(
            IFileItemService fileItemService,
            ITranscribeItemService transcribeItemService,
            IUserSubscriptionService userSubscriptionService)
        {
            _fileItemService = fileItemService;
            _transcribeItemService = transcribeItemService;
            _userSubscriptionService = userSubscriptionService;
        }

        [HttpGet("/api/last-updates")]
        [ProducesResponseType(typeof(LastUpdatesDto), StatusCodes.Status200OK)]
        [SwaggerOperation(OperationId = "GetLastUpdates")]
        public async Task<ActionResult> Get()
        {
            var userId = HttpContext.User.GetNameIdentifier();

            var fileItemLastUpdate = await _fileItemService.GetLastUpdateAsync(userId).ConfigureAwait(false);
            var deletedFileItemLastUpdate = await _fileItemService.GetDeletedLastUpdateAsync(userId).ConfigureAwait(false);
            var transcribeItemLastUpdate = await _transcribeItemService.GetLastUpdateAsync(userId).ConfigureAwait(false);
            var userSubscriptionUpdate = await _userSubscriptionService.GetLastUpdateAsync(userId).ConfigureAwait(false);

            return Ok(new LastUpdatesDto
            {
                FileItem = fileItemLastUpdate,
                DeletedFileItem = deletedFileItemLastUpdate,
                TranscribeItem = transcribeItemLastUpdate,
                UserSubscription = userSubscriptionUpdate
            });
        }
    }
}