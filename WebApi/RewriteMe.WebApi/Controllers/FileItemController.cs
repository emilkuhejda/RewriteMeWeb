using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RewriteMe.Domain.Enums;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.Domain.Managers;
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
        private readonly ISpeechRecognitionManager _speechRecognitionManager;

        public FileItemController(
            IFileItemService fileItemService,
            ISpeechRecognitionManager speechRecognitionManager,
            ISpeechRecognitionService speechRecognitionService)
        {
            _fileItemService = fileItemService;
            _speechRecognitionManager = speechRecognitionManager;
        }

        [HttpGet("/api/files")]
        public async Task<IActionResult> Get()
        {
            var userId = Guid.Parse(HttpContext.User.Identity.Name);
            var files = await _fileItemService.GetAllAsync(userId).ConfigureAwait(false);

            return Ok(files.Select(x => new
            {
                x.Id,
                x.Name,
                x.FileName,
                x.RecognitionState,
                x.DateCreated,
                x.DateProcessed
            }));
        }

        [HttpGet("/api/files/{fileItemId}")]
        public async Task<IActionResult> Get(Guid fileItemId)
        {
            var userId = Guid.Parse(HttpContext.User.Identity.Name);
            var file = await _fileItemService.GetFileItemWithoutSourceAsync(userId, fileItemId).ConfigureAwait(false);

            return Ok(new
            {
                Id = file.Id,
                Name = file.Name,
                FileName = file.FileName,
                RecognitionState = file.RecognitionState,
                DateCreated = file.DateCreated,
                DateProcessed = file.DateProcessed
            });
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
                    ContentType = fileToUpload.ContentType,
                    DateCreated = DateTime.UtcNow
                };

                await _fileItemService.AddAsync(fileItem).ConfigureAwait(false);
            }

            return Ok();
        }

        [HttpPut("/api/files/update")]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> Update([FromForm] UploadFileModel uploadFileModel)
        {
            var files = Request.Form.Files;
            if (files.Count > 1)
                return StatusCode(416);

            var fileItem = new FileItem
            {
                Id = uploadFileModel.FileItemId,
                UserId = Guid.Parse(HttpContext.User.Identity.Name),
                Name = uploadFileModel.Name
            };

            if (files.Any())
            {
                using (var memoryStream = new MemoryStream())
                {
                    var fileToUpoad = files.First();
                    fileToUpoad.CopyTo(memoryStream);

                    fileItem.FileName = fileToUpoad.Name;
                    fileItem.Source = memoryStream.ToArray();
                    fileItem.ContentType = fileToUpoad.ContentType;
                }
            }

            await _fileItemService.UpdateAsync(fileItem).ConfigureAwait(false);

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

        [HttpPost("/api/files/transcribe")]
        public async Task<IActionResult> Transcribe([FromBody] TranscribeFileItemModel transcribeFileItemModel)
        {
            var userId = Guid.Parse(HttpContext.User.Identity.Name);
            var fileItem = await _fileItemService.GetFileItemWithoutTranscriptionAsync(userId, transcribeFileItemModel.FileItemId).ConfigureAwait(false);

            if (fileItem.RecognitionState != RecognitionState.None)
                return BadRequest();

            BackgroundJob.Enqueue(() => _speechRecognitionManager.RunRecognition(fileItem));

            return Ok();
        }
    }
}