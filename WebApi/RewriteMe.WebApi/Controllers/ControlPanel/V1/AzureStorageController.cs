using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RewriteMe.Domain.Enums;
using RewriteMe.Domain.Interfaces.Services;

namespace RewriteMe.WebApi.Controllers.ControlPanel.V1
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/control-panel/azure-storage")]
    [Produces("application/json")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize(Roles = nameof(Role.Admin))]
    [ApiController]
    [AllowAnonymous]
    public class AzureStorageController : ControllerBase
    {
        private readonly IStorageService _storageService;

        public AzureStorageController(IStorageService storageService)
        {
            _storageService = storageService;
        }

        [HttpPatch("migration")]
        public IActionResult Migration()
        {
            var monitoringApi = JobStorage.Current.GetMonitoringApi();
            if (monitoringApi.ProcessingCount() > 0)
                return BadRequest();

            BackgroundJob.Enqueue(() => _storageService.Migrate());

            return Ok();
        }
    }
}