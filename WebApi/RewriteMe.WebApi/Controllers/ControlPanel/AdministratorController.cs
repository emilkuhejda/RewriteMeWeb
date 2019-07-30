using System;
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

        [HttpGet("/control-panel/administrators/{administratorId}")]
        public async Task<IActionResult> Get(Guid administratorId)
        {
            var administrators = await _administratorService.GetAsync(administratorId).ConfigureAwait(false);
            return Ok(administrators);
        }

        [HttpGet("/control-panel/administrators")]
        public async Task<IActionResult> GetAll()
        {
            var administrators = await _administratorService.GetAllAsync().ConfigureAwait(false);
            return Ok(administrators);
        }

        [HttpPost("/control-panel/administrators/create")]
        public async Task<IActionResult> Create(CreateAdministratorModel createAdministratorModel)
        {
            var administrator = createAdministratorModel.ToAdministrator();
            var alreadyExists = await _administratorService.AlreadyExists(administrator).ConfigureAwait(false);
            if (alreadyExists)
                return StatusCode(409);

            await _administratorService.AddAsync(administrator).ConfigureAwait(false);

            return Ok();
        }

        [HttpPut("/control-panel/administrators/update")]
        public async Task<IActionResult> Update(UpdateAdministratorModel updateAdministrator)
        {
            var administrator = updateAdministrator.ToAdministrator();
            var alreadyExists = await _administratorService.AlreadyExists(administrator).ConfigureAwait(false);
            if (alreadyExists)
                return StatusCode(409);

            await _administratorService.UpdateAsync(administrator).ConfigureAwait(false);

            return Ok();
        }

        [HttpDelete("/control-panel/administrators/delete")]
        public async Task<IActionResult> Delete(Guid administratorId)
        {
            await _administratorService.DeleteAsync(administratorId).ConfigureAwait(false);

            return Ok();
        }
    }
}