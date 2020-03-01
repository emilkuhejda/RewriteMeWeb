using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RewriteMe.Domain.Dtos;
using RewriteMe.Domain.Enums;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.WebApi.Extensions;
using Swashbuckle.AspNetCore.Annotations;

namespace RewriteMe.WebApi.Controllers.V1
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/last-updates")]
    [Produces("application/json")]
    [Authorize(Roles = nameof(Role.User))]
    [ApiController]
    public class LastUpdatesController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IFileItemService _fileItemService;
        private readonly ITranscribeItemService _transcribeItemService;
        private readonly IUserSubscriptionService _userSubscriptionService;
        private readonly IInformationMessageService _informationMessageService;

        public LastUpdatesController(
            IUserService userService,
            IFileItemService fileItemService,
            ITranscribeItemService transcribeItemService,
            IUserSubscriptionService userSubscriptionService,
            IInformationMessageService informationMessageService)
        {
            _userService = userService;
            _fileItemService = fileItemService;
            _transcribeItemService = transcribeItemService;
            _userSubscriptionService = userSubscriptionService;
            _informationMessageService = informationMessageService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(LastUpdatesDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(OperationId = "GetLastUpdates")]
        public async Task<ActionResult> Get()
        {
            var userId = HttpContext.User.GetNameIdentifier();
            var user = await _userService.GetAsync(userId).ConfigureAwait(false);
            if (user == null)
                return Unauthorized();

            var fileItemLastUpdate = await _fileItemService.GetLastUpdateAsync(userId).ConfigureAwait(false);
            var deletedFileItemLastUpdate = await _fileItemService.GetDeletedLastUpdateAsync(userId).ConfigureAwait(false);
            var transcribeItemLastUpdate = await _transcribeItemService.GetLastUpdateAsync(userId).ConfigureAwait(false);
            var userSubscriptionUpdate = await _userSubscriptionService.GetLastUpdateAsync(userId).ConfigureAwait(false);
            var informationMessageUpdate = await _informationMessageService.GetLastUpdateAsync(userId).ConfigureAwait(false);

            return Ok(new LastUpdatesDto
            {
                FileItemUtc = fileItemLastUpdate,
                DeletedFileItemUtc = deletedFileItemLastUpdate,
                TranscribeItemUtc = transcribeItemLastUpdate,
                UserSubscriptionUtc = userSubscriptionUpdate,
                InformationMessageUtc = informationMessageUpdate
            });
        }
    }
}