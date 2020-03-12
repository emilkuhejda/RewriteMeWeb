using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RewriteMe.Domain.Enums;
using RewriteMe.WebApi.Commands;

namespace RewriteMe.WebApi.Controllers.ControlPanel.V1
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/control-panel/azure-storage")]
    [Produces("application/json")]
    //[ApiExplorerSettings(IgnoreApi = true)]
    [Authorize(Roles = nameof(Role.Admin))]
    [ApiController]
    [AllowAnonymous]
    public class AzureStorageController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AzureStorageController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPatch("migration")]
        public async Task<IActionResult> Migration()
        {
            var okDto = await _mediator.Send(new MigrateFilesCommand()).ConfigureAwait(false);

            return Ok(okDto);
        }
    }
}