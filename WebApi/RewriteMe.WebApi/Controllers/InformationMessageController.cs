using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RewriteMe.Domain.Enums;
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
    public class InformationMessageController : ControllerBase
    {
        private readonly IInformationMessageService _informationMessageService;

        public InformationMessageController(IInformationMessageService informationMessageService)
        {
            _informationMessageService = informationMessageService;
        }

        [HttpGet("/api/information-messages")]
        [Authorize(Roles = nameof(Role.User))]
        [ProducesResponseType(typeof(IEnumerable<InformationMessageDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [SwaggerOperation(OperationId = "GetInformationMessages")]
        public async Task<IActionResult> GetAll(DateTime updatedAfter)
        {
            var informationMessages = await _informationMessageService.GetAllAsync(updatedAfter.ToUniversalTime()).ConfigureAwait(false);

            return Ok(informationMessages.Select(x => x.ToDto()));
        }
    }
}