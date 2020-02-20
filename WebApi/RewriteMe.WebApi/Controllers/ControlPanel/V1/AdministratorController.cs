using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RewriteMe.Domain.Enums;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.WebApi.Extensions;
using RewriteMe.WebApi.Models;

namespace RewriteMe.WebApi.Controllers.ControlPanel.V1
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/control-panel/administrators")]
    [Produces("application/json")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize(Roles = nameof(Role.Admin))]
    [ApiController]
    public class AdministratorController : ControllerBase
    {
        private readonly IAdministratorService _administratorService;

        public AdministratorController(IAdministratorService administratorService)
        {
            _administratorService = administratorService;
        }

        [HttpGet("{administratorId}")]
        public async Task<IActionResult> Get(Guid administratorId)
        {
            var administrators = await _administratorService.GetAsync(administratorId).ConfigureAwait(false);
            return Ok(administrators);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var administrators = await _administratorService.GetAllAsync().ConfigureAwait(false);
            return Ok(administrators);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(CreateAdministratorModel createAdministratorModel)
        {
            var administrator = createAdministratorModel.ToAdministrator();
            var alreadyExists = await _administratorService.AlreadyExistsAsync(administrator).ConfigureAwait(false);
            if (alreadyExists)
                return StatusCode(409);

            await _administratorService.AddAsync(administrator).ConfigureAwait(false);

            return Ok();
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update(UpdateAdministratorModel updateAdministrator)
        {
            var administrator = updateAdministrator.ToAdministrator();
            var alreadyExists = await _administratorService.AlreadyExistsAsync(administrator).ConfigureAwait(false);
            if (alreadyExists)
                return StatusCode(409);

            await _administratorService.UpdateAsync(administrator).ConfigureAwait(false);

            return Ok();
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete(Guid administratorId)
        {
            await _administratorService.DeleteAsync(administratorId).ConfigureAwait(false);

            return Ok();
        }
    }
}