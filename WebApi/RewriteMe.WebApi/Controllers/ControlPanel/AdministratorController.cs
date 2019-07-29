using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.WebApi.Extensions;
using RewriteMe.WebApi.Models;

namespace RewriteMe.WebApi.Controllers.ControlPanel
{
    [Route("control-panel/[controller]")]
    [Produces("application/json")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize]
    [ApiController]
    public class AdministratorController : ControllerBase
    {
        private readonly IAdministratorService _administratorService;

        public AdministratorController(IAdministratorService administratorService)
        {
            _administratorService = administratorService;
        }

        [HttpGet("/control-panel/administrators")]
        public async Task<IActionResult> GetAll()
        {
            var administrators = await _administratorService.GetAllAsync().ConfigureAwait(false);
            return Ok(administrators);
        }

        [HttpPost("/control-panel/administrators/create")]
        public async Task<IActionResult> Create([FromForm] CreateAdministratorModel createAdministratorModel)
        {
            var administrator = createAdministratorModel.ToAdministrator();
            await _administratorService.AddAsync(administrator).ConfigureAwait(false);

            return Ok();
        }
    }
}