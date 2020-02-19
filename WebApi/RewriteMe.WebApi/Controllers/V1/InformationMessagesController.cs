using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RewriteMe.Domain.Dtos;
using RewriteMe.Domain.Enums;
using RewriteMe.Domain.Extensions;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.WebApi.Extensions;
using Swashbuckle.AspNetCore.Annotations;

namespace RewriteMe.WebApi.Controllers.V1
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/information-messages")]
    [Produces("application/json")]
    [Authorize(Roles = nameof(Role.User))]
    [ApiController]
    public class InformationMessagesController : ControllerBase
    {
        private readonly IInformationMessageService _informationMessageService;

        public InformationMessagesController(IInformationMessageService informationMessageService)
        {
            _informationMessageService = informationMessageService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<InformationMessageDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(OperationId = "GetInformationMessages")]
        public async Task<IActionResult> GetAll(DateTime updatedAfter)
        {
            var userId = HttpContext.User.GetNameIdentifier();
            var informationMessages = await _informationMessageService.GetAllAsync(userId, updatedAfter.ToUniversalTime()).ConfigureAwait(false);

            return Ok(informationMessages.Select(x => x.ToDto()));
        }

        [HttpGet("{informationMessageId}")]
        [ProducesResponseType(typeof(InformationMessageDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> Get(Guid informationMessageId)
        {
            var informationMessage = await _informationMessageService.GetAsync(informationMessageId).ConfigureAwait(false);
            return Ok(informationMessage.ToDto());
        }

        [HttpPut("mark-as-opened")]
        [ProducesResponseType(typeof(InformationMessageDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(OperationId = "MarkMessageAsOpened")]
        public async Task<IActionResult> MarkAsOpened(Guid informationMessageId)
        {
            var userId = HttpContext.User.GetNameIdentifier();
            var informationMessage = await _informationMessageService.MarkAsOpenedAsync(userId, informationMessageId).ConfigureAwait(false);
            if (informationMessage == null)
                return BadRequest();

            return Ok(informationMessage.ToDto());
        }

        [HttpPut("mark-messages-as-opened")]
        [ProducesResponseType(typeof(OkDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(OperationId = "MarkMessagesAsOpened")]
        public async Task<IActionResult> MarkMessagesAsOpened(IEnumerable<Guid> ids)
        {
            var userId = HttpContext.User.GetNameIdentifier();
            await _informationMessageService.MarkAsOpenedAsync(userId, ids).ConfigureAwait(false);
            return Ok(new OkDto());
        }
    }
}