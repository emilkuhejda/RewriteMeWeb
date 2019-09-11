using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RewriteMe.Domain.Enums;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.WebApi.Dtos;
using Swashbuckle.AspNetCore.Annotations;

namespace RewriteMe.WebApi.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Authorize(Roles = nameof(Role.User))]
    [Authorize]
    [ApiController]
    public class LastUpdatesController : RewriteMeControllerBase
    {
        private readonly IFileItemService _fileItemService;
        private readonly ITranscribeItemService _transcribeItemService;
        private readonly IUserSubscriptionService _userSubscriptionService;
        private readonly IInformationMessageService _informationMessageService;

        public LastUpdatesController(
            IFileItemService fileItemService,
            ITranscribeItemService transcribeItemService,
            IUserSubscriptionService userSubscriptionService,
            IInformationMessageService informationMessageService,
            IUserService userService)
            : base(userService)
        {
            _fileItemService = fileItemService;
            _transcribeItemService = transcribeItemService;
            _userSubscriptionService = userSubscriptionService;
            _informationMessageService = informationMessageService;
        }

        [HttpGet("/api/last-updates")]
        [ProducesResponseType(typeof(LastUpdatesDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [SwaggerOperation(OperationId = "GetLastUpdates")]
        public async Task<ActionResult> Get()
        {
            var user = await VerifyUserAsync().ConfigureAwait(false);
            if (user == null)
                return StatusCode(401);

            var fileItemLastUpdate = await _fileItemService.GetLastUpdateAsync(user.Id).ConfigureAwait(false);
            var deletedFileItemLastUpdate = await _fileItemService.GetDeletedLastUpdateAsync(user.Id).ConfigureAwait(false);
            var transcribeItemLastUpdate = await _transcribeItemService.GetLastUpdateAsync(user.Id).ConfigureAwait(false);
            var userSubscriptionUpdate = await _userSubscriptionService.GetLastUpdateAsync(user.Id).ConfigureAwait(false);
            var informationMessageUpdate = await _informationMessageService.GetLastUpdateAsync(user.Id).ConfigureAwait(false);

            return Ok(new LastUpdatesDto
            {
                FileItem = fileItemLastUpdate,
                DeletedFileItem = deletedFileItemLastUpdate,
                TranscribeItem = transcribeItemLastUpdate,
                UserSubscription = userSubscriptionUpdate,
                InformationMessage = informationMessageUpdate
            });
        }
    }
}