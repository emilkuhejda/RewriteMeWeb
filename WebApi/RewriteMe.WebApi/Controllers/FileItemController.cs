using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.Domain.Settings;
using RewriteMe.Domain.Transcription;
using RewriteMe.WebApi.Extensions;
using RewriteMe.WebApi.Models;

namespace RewriteMe.WebApi.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Authorize]
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

        [HttpGet("/api/files")]
        public async Task<IActionResult> Get()
        {
            var userId = Guid.Parse(HttpContext.User.Identity.Name);
            var files = await _fileItemService.GetAllAsync(userId).ConfigureAwait(false);

            return Ok(files);
        }

        [HttpPost("/api/files/create")]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> Create([FromForm] CreateFileModel createFileModel)
        {
            var files = Request.Form.Files;
            if (!files.Any())
                return BadRequest();

            if (files.Count > 1)
                return StatusCode(416);

            var fileToUpload = files[0];
            if (!fileToUpload.IsSupportedType())
                return StatusCode(415);

            using (var memoryStream = new MemoryStream())
            {
                fileToUpload.CopyTo(memoryStream);

                var fileItem = new FileItem
                {
                    Id = Guid.NewGuid(),
                    UserId = Guid.Parse(HttpContext.User.Identity.Name),
                    Name = createFileModel.Name,
                    FileName = fileToUpload.Name,
                    Source = memoryStream.ToArray(),
                    DateCreated = DateTime.UtcNow,
                };

                await _fileItemService.AddAsync(fileItem).ConfigureAwait(false);
            }

            return Ok();
        }

        [HttpDelete("/api/files/{id}")]
        public async Task<IActionResult> Remove([FromRoute] string id)
        {
            var userId = Guid.Parse(HttpContext.User.Identity.Name);
            var fileItemId = Guid.Parse(id);

            await _fileItemService.RemoveAsync(userId, fileItemId).ConfigureAwait(false);

            return Ok();
        }
    }
}