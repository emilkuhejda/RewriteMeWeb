using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RewriteMe.Common.Utils;
using RewriteMe.Domain.Dtos;
using RewriteMe.Domain.Enums;
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
        private readonly IApplicationLogService _applicationLogService;

        public InformationMessagesController(
            IInformationMessageService informationMessageService,
            IApplicationLogService applicationLogService)
        {
            _informationMessageService = informationMessageService;
            _applicationLogService = applicationLogService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<InformationMessageDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(OperationId = "GetInformationMessages")]
        public async Task<IActionResult> GetAll(DateTime updatedAfter)
        {
            try
            {
                var userId = HttpContext.User.GetNameIdentifier();
                var informationMessages = await _informationMessageService.GetAllAsync(userId, updatedAfter.ToUniversalTime()).ConfigureAwait(false);

                return Ok(informationMessages.Select(x => x.ToDto()));
            }
            catch (Exception ex)
            {
                await _applicationLogService.ErrorAsync($"{ExceptionFormatter.FormatException(ex)}").ConfigureAwait(false);
            }

            return StatusCode((int)HttpStatusCode.InternalServerError);
        }

        [HttpGet("{informationMessageId}")]
        [ProducesResponseType(typeof(InformationMessageDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> Get(Guid informationMessageId)
        {
            try
            {
                var informationMessage = await _informationMessageService.GetAsync(informationMessageId).ConfigureAwait(false);
                return Ok(informationMessage.ToDto());
            }
            catch (Exception ex)
            {
                await _applicationLogService.ErrorAsync($"{ExceptionFormatter.FormatException(ex)}").ConfigureAwait(false);
            }

            return StatusCode((int)HttpStatusCode.InternalServerError);
        }

        [HttpPut("mark-as-opened")]
        [ProducesResponseType(typeof(InformationMessageDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(OperationId = "MarkMessageAsOpened")]
        public async Task<IActionResult> MarkAsOpened(Guid informationMessageId)
        {
            try
            {
                var userId = HttpContext.User.GetNameIdentifier();
                var informationMessage = await _informationMessageService.MarkAsOpenedAsync(userId, informationMessageId).ConfigureAwait(false);
                if (informationMessage == null)
                    return BadRequest();

                return Ok(informationMessage.ToDto());
            }
            catch (Exception ex)
            {
                await _applicationLogService.ErrorAsync($"{ExceptionFormatter.FormatException(ex)}").ConfigureAwait(false);
            }

            return StatusCode((int)HttpStatusCode.InternalServerError);
        }

        [HttpPut("mark-messages-as-opened")]
        [ProducesResponseType(typeof(OkDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(OperationId = "MarkMessagesAsOpened")]
        public async Task<IActionResult> MarkMessagesAsOpened(IEnumerable<Guid> ids)
        {
            try
            {
                var userId = HttpContext.User.GetNameIdentifier();
                await _informationMessageService.MarkAsOpenedAsync(userId, ids).ConfigureAwait(false);
                return Ok(new OkDto());
            }
            catch (Exception ex)
            {
                await _applicationLogService.ErrorAsync($"{ExceptionFormatter.FormatException(ex)}").ConfigureAwait(false);
            }

            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }
}