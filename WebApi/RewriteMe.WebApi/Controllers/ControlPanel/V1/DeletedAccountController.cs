using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RewriteMe.Domain.Enums;
using RewriteMe.Domain.Interfaces.Services;

namespace RewriteMe.WebApi.Controllers.ControlPanel.V1
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/control-panel/deleted-accounts")]
    [Produces("application/json")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize(Roles = nameof(Role.Admin))]
    [ApiController]
    public class DeletedAccountController : ControllerBase
    {
        private readonly IDeletedAccountService _deletedAccountService;

        public DeletedAccountController(IDeletedAccountService deletedAccountService)
        {
            _deletedAccountService = deletedAccountService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var deletedAccounts = await _deletedAccountService.GetAllAsync().ConfigureAwait(false);

            return Ok(deletedAccounts);
        }

        [HttpDelete("{deletedAccountId}")]
        public async Task<IActionResult> Delete(Guid deletedAccountId)
        {
            await _deletedAccountService.DeleteAsync(deletedAccountId).ConfigureAwait(false);

            return Ok();
        }
    }
}