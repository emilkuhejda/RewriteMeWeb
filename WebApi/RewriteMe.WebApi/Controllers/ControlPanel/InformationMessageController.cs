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

namespace RewriteMe.WebApi.Controllers.ControlPanel
{
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

        [HttpGet("/api/control-panel/information-messages")]
        public async Task<IActionResult> GetAll()
        {
            var informationMessages = await _informationMessageService.GetAllAsync(default(DateTime)).ConfigureAwait(false);
            return Ok(informationMessages);
        }

        [HttpPost("/api/control-panel/information-messages/create")]
        public async Task<IActionResult> Create([FromForm]InformationMessageModel informationMessageModel)
        {
            var informationMessage = informationMessageModel.ToInformationMessage(Guid.NewGuid());
            await _informationMessageService.AddAsync(informationMessage).ConfigureAwait(false);

            return Ok();
        }

        [HttpPut("/api/control-panel/information-messages/{informationMessageId}")]
        public async Task<IActionResult> Update(Guid informationMessageId, [FromForm]InformationMessageModel informationMessageModel)
        {
            var informationMessage = informationMessageModel.ToInformationMessage(informationMessageId);
            await _informationMessageService.UpdateAsync(informationMessage).ConfigureAwait(false);

            return Ok(informationMessage);
        }

        [HttpPut("/api/control-panel/information-messages/send")]
        public async Task<IActionResult> SendNotifications([FromForm]Guid informationMessageId, [FromForm]RuntimePlatform runtimePlatform, [FromForm]Language language)
        {
            if (language == Language.Undefined)
                return StatusCode(406);

            var informationMessage = await _informationMessageService.GetAsync(informationMessageId).ConfigureAwait(false);
            if (informationMessage == null)
                return BadRequest();

            try
            {
                await _applicationLogService.InfoAsync($"Sending notification with ID = '{informationMessage.Id}'").ConfigureAwait(false);

                var notificationResult = await _pushNotificationsService.SendAsync(informationMessage, runtimePlatform, language).ConfigureAwait(false);
                if (notificationResult == null)
                    return BadRequest();

                return Ok();
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
            catch (EmptyDeviceListException)
            {
                return StatusCode(409);
            }
        }
    }
}