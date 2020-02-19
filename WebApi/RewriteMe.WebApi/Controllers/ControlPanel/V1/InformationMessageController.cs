using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Rest;
using RewriteMe.Common.Utils;
using RewriteMe.Domain.Enums;
using RewriteMe.Domain.Exceptions;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.WebApi.Extensions;
using RewriteMe.WebApi.Models;

namespace RewriteMe.WebApi.Controllers.ControlPanel.V1
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/control-panel/information-messages")]
    [Produces("application/json")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize(Roles = nameof(Role.Admin))]
    [ApiController]
    public class InformationMessageController : ControllerBase
    {
        private readonly IInformationMessageService _informationMessageService;
        private readonly IPushNotificationsService _pushNotificationsService;
        private readonly IApplicationLogService _applicationLogService;

        public InformationMessageController(
            IInformationMessageService informationMessageService,
            IPushNotificationsService pushNotificationsService,
            IApplicationLogService applicationLogService)
        {
            _informationMessageService = informationMessageService;
            _pushNotificationsService = pushNotificationsService;
            _applicationLogService = applicationLogService;
        }

        [HttpGet("{informationMessageId}")]
        public async Task<IActionResult> Get(Guid informationMessageId)
        {
            var informationMessage = await _informationMessageService.GetAsync(informationMessageId).ConfigureAwait(false);
            return Ok(informationMessage);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var informationMessages = await _informationMessageService.GetAllShallowAsync().ConfigureAwait(false);
            return Ok(informationMessages);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromForm]InformationMessageModel informationMessageModel)
        {
            var informationMessage = informationMessageModel.ToInformationMessage(Guid.NewGuid());
            await _informationMessageService.AddAsync(informationMessage).ConfigureAwait(false);

            return Ok(informationMessage.Id);
        }

        [HttpPut("{informationMessageId}")]
        public async Task<IActionResult> Update(Guid informationMessageId, [FromForm]InformationMessageModel informationMessageModel)
        {
            var informationMessage = informationMessageModel.ToInformationMessage(informationMessageId);
            var canUpdate = await _informationMessageService.CanUpdateAsync(informationMessageId).ConfigureAwait(false);
            if (!canUpdate)
                return BadRequest();

            await _informationMessageService.UpdateAsync(informationMessage).ConfigureAwait(false);

            return Ok(informationMessage);
        }

        [HttpPut("send")]
        public async Task<IActionResult> SendNotification([FromForm]Guid informationMessageId, [FromForm]RuntimePlatform runtimePlatform, [FromForm]Language language)
        {
            try
            {
                if (language == Language.Undefined)
                    return StatusCode(406);

                var informationMessage = await _informationMessageService.GetAsync(informationMessageId).ConfigureAwait(false);
                if (informationMessage == null)
                    return BadRequest();

                await _applicationLogService.InfoAsync($"Sending notification with ID = '{informationMessage.Id}'").ConfigureAwait(false);
                await _pushNotificationsService.SendAsync(informationMessage, runtimePlatform, language).ConfigureAwait(false);

                informationMessage = await _informationMessageService.GetAsync(informationMessageId).ConfigureAwait(false);
                return Ok(informationMessage);
            }
            catch (SerializationException ex)
            {
                await _applicationLogService.ErrorAsync($"Request exception during sending notification with message: '{ex.Message}'").ConfigureAwait(false);
                await _applicationLogService.ErrorAsync(ExceptionFormatter.FormatException(ex)).ConfigureAwait(false);

                return StatusCode(500);
            }
            catch (NotificationErrorException ex)
            {
                await _applicationLogService.ErrorAsync($"Request exception during sending notification with message: '{ex.NotificationError.Message}'").ConfigureAwait(false);
                await _applicationLogService.ErrorAsync(ExceptionFormatter.FormatException(ex)).ConfigureAwait(false);

                return StatusCode((int)ex.NotificationError.Code);
            }
            catch (LanguageVersionNotExistsException)
            {
                await _applicationLogService.ErrorAsync($"Language version not found for information message with ID = '{informationMessageId}'.").ConfigureAwait(false);

                return NotFound();
            }
            catch (PushNotificationWasSentException)
            {
                await _applicationLogService.ErrorAsync($"Push notification was already sent for information message with ID = '{informationMessageId}'.").ConfigureAwait(false);

                return StatusCode(409);
            }
        }
    }
}