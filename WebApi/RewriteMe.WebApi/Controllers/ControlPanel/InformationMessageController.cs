using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RewriteMe.Domain.Enums;
using RewriteMe.Domain.Interfaces.Services;

namespace RewriteMe.WebApi.Controllers.ControlPanel
{
    [Produces("application/json")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize(Roles = nameof(Role.Admin))]
    [ApiController]
    public class InformationMessageController : ControllerBase
    {
        private readonly IInformationMessageService _informationMessageService;

        public InformationMessageController(IInformationMessageService informationMessageService)
        {
            _informationMessageService = informationMessageService;
        }

        [HttpGet("/api/control-panel/information-messages")]
        public async Task<IActionResult> GetAll()
        {
            var informationMessages = await _informationMessageService.GetAllAsync(default(DateTime)).ConfigureAwait(false);
            return Ok(informationMessages);
        }
    }
}