using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RewriteMe.Common.Utils;
using RewriteMe.Domain.Enums;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.WebApi.Extensions;
using RewriteMe.WebApi.Models;

namespace RewriteMe.WebApi.Controllers.ControlPanel
{
    [Produces("application/json")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize(Roles = nameof(Role.Admin))]
    [ApiController]
    public class AdministratorController : ControllerBase
    {
        private readonly IAdministratorService _administratorService;
        private readonly IApplicationLogService _applicationLogService;

        public AdministratorController(
            IAdministratorService administratorService,
            IApplicationLogService applicationLogService)
        {
            _administratorService = administratorService;
            _applicationLogService = applicationLogService;
        }

        [HttpGet("/api/control-panel/administrators/{administratorId}")]
        public async Task<IActionResult> Get(Guid administratorId)
        {
            try
            {
                var administrators = await _administratorService.GetAsync(administratorId).ConfigureAwait(false);
                return Ok(administrators);
            }
            catch (Exception ex)
            {
                await _applicationLogService.ErrorAsync($"{ExceptionFormatter.FormatException(ex)}").ConfigureAwait(false);
            }

            return StatusCode((int)HttpStatusCode.InternalServerError);
        }

        [HttpGet("/api/control-panel/administrators")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var administrators = await _administratorService.GetAllAsync().ConfigureAwait(false);
                return Ok(administrators);
            }
            catch (Exception ex)
            {
                await _applicationLogService.ErrorAsync($"{ExceptionFormatter.FormatException(ex)}").ConfigureAwait(false);
            }

            return StatusCode((int)HttpStatusCode.InternalServerError);
        }

        [HttpPost("/api/control-panel/administrators/create")]
        public async Task<IActionResult> Create(CreateAdministratorModel createAdministratorModel)
        {
            try
            {
                var administrator = createAdministratorModel.ToAdministrator();
                var alreadyExists = await _administratorService.AlreadyExistsAsync(administrator).ConfigureAwait(false);
                if (alreadyExists)
                    return StatusCode(409);

                await _administratorService.AddAsync(administrator).ConfigureAwait(false);

                return Ok();
            }
            catch (Exception ex)
            {
                await _applicationLogService.ErrorAsync($"{ExceptionFormatter.FormatException(ex)}").ConfigureAwait(false);
            }

            return StatusCode((int)HttpStatusCode.InternalServerError);
        }

        [HttpPut("/api/control-panel/administrators/update")]
        public async Task<IActionResult> Update(UpdateAdministratorModel updateAdministrator)
        {
            try
            {
                var administrator = updateAdministrator.ToAdministrator();
                var alreadyExists = await _administratorService.AlreadyExistsAsync(administrator).ConfigureAwait(false);
                if (alreadyExists)
                    return StatusCode(409);

                await _administratorService.UpdateAsync(administrator).ConfigureAwait(false);

                return Ok();
            }
            catch (Exception ex)
            {
                await _applicationLogService.ErrorAsync($"{ExceptionFormatter.FormatException(ex)}").ConfigureAwait(false);
            }

            return StatusCode((int)HttpStatusCode.InternalServerError);
        }

        [HttpDelete("/api/control-panel/administrators/delete")]
        public async Task<IActionResult> Delete(Guid administratorId)
        {
            try
            {
                await _administratorService.DeleteAsync(administratorId).ConfigureAwait(false);

                return Ok();
            }
            catch (Exception ex)
            {
                await _applicationLogService.ErrorAsync($"{ExceptionFormatter.FormatException(ex)}").ConfigureAwait(false);
            }

            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }
}