using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RewriteMe.Common.Utils;
using RewriteMe.Domain.Enums;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.WebApi.Dtos;
using Swashbuckle.AspNetCore.Annotations;

namespace RewriteMe.WebApi.Controllers.V1
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/last-updates")]
    [Produces("application/json")]
    [Authorize(Roles = nameof(Role.User))]
    [ApiController]
    public class LastUpdatesController : RewriteMeControllerBase
    {
        private readonly IFileItemService _fileItemService;
        private readonly ITranscribeItemService _transcribeItemService;
        private readonly IUserSubscriptionService _userSubscriptionService;
        private readonly IInformationMessageService _informationMessageService;
        private readonly IApplicationLogService _applicationLogService;

        public LastUpdatesController(
            IFileItemService fileItemService,
            ITranscribeItemService transcribeItemService,
            IUserSubscriptionService userSubscriptionService,
            IInformationMessageService informationMessageService,
            IApplicationLogService applicationLogService,
            IUserService userService)
            : base(userService)
        {
            _fileItemService = fileItemService;
            _transcribeItemService = transcribeItemService;
            _userSubscriptionService = userSubscriptionService;
            _informationMessageService = informationMessageService;
            _applicationLogService = applicationLogService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(LastUpdatesDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(OperationId = "GetLastUpdates")]
        public async Task<ActionResult> Get()
        {
            try
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
                    FileItemUtc = fileItemLastUpdate,
                    DeletedFileItemUtc = deletedFileItemLastUpdate,
                    TranscribeItemUtc = transcribeItemLastUpdate,
                    UserSubscriptionUtc = userSubscriptionUpdate,
                    InformationMessageUtc = informationMessageUpdate
                });
            }
            catch (Exception ex)
            {
                await _applicationLogService.ErrorAsync($"{ExceptionFormatter.FormatException(ex)}").ConfigureAwait(false);
            }

            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }
}