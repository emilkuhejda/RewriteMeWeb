using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RewriteMe.Domain.Enums;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.Domain.Settings;

namespace RewriteMe.WebApi.Controllers.ControlPanel
{
    [Produces("application/json")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize(Roles = nameof(Role.Admin))]
    [ApiController]
    public class FileItemController : ControllerBase
    {
        private readonly IFileItemService _fileItemService;
        private readonly AppSettings _appSettings;

        public FileItemController(
            IFileItemService fileItemService,
            IOptions<AppSettings> options)
        {
            _fileItemService = fileItemService;
            _appSettings = options.Value;
        }

        [HttpGet("/api/control-panel/files/{userId}")]
        public async Task<IActionResult> Get(Guid userId)
        {
            var fileItems = await _fileItemService.GetAllForUserAsync(userId).ConfigureAwait(false);

            return Ok(fileItems);
        }

        [HttpPut("/api/control-panel/files/restore")]
        public async Task<IActionResult> Restore(Guid userId, Guid fileItemId)
        {
            await _fileItemService.RestoreAllAsync(userId, new[] { fileItemId }, _appSettings.ApplicationId).ConfigureAwait(false);

            return Ok();
        }
    }
}