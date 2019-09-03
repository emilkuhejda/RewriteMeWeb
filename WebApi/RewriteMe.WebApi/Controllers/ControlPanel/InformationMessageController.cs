using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RewriteMe.Domain.Enums;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.WebApi.Extensions;
using RewriteMe.WebApi.Models;

namespace RewriteMe.WebApi.Controllers.ControlPanel
{
    [Produces("application/json")]
    //[ApiExplorerSettings(IgnoreApi = true)]
    [Authorize(Roles = nameof(Role.Admin))]
    [ApiController]
    public class InformationMessageController : ControllerBase
    {
        private readonly IInformationMessageService _informationMessageService;
        private readonly IPushNotificationsService _pushNotificationsService;

        public InformationMessageController(
            IInformationMessageService informationMessageService,
            IPushNotificationsService pushNotificationsService)
        {
            _informationMessageService = informationMessageService;
            _pushNotificationsService = pushNotificationsService;
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

            return Ok();
        }

        [HttpPut("/api/control-panel/information-messages/send")]
        public async Task<IActionResult> SendNotifications([FromForm]Guid informationMessage, [FromForm]Language language)
        {
            var id = informationMessage;
            var lang = language;

            return Ok();
        }
    }
}