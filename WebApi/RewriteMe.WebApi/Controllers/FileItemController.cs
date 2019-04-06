using System;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RewriteMe.Domain;
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
        private readonly IAudioSourceService _audioSourceService;
        private readonly ISpeechRecognitionManager _speechRecognitionManager;
        private readonly IWavFileManager _wavFileManager;

        public FileItemController(
            IFileItemService fileItemService,
            IAudioSourceService audioSourceService,
            ISpeechRecognitionManager speechRecognitionManager,
            IWavFileManager wavFileManager)
        {
            _fileItemService = fileItemService;
            _audioSourceService = audioSourceService;
            _speechRecognitionManager = speechRecognitionManager;
            _wavFileManager = wavFileManager;
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
                x.Language,
                x.RecognitionState,
                x.DateCreated,
                x.DateProcessed
            }));
        }

        [HttpGet("/api/files/{fileItemId}")]
        public async Task<IActionResult> Get(Guid fileItemId)
        {
            var userId = Guid.Parse(HttpContext.User.Identity.Name);
            var file = await _fileItemService.GetAsync(userId, fileItemId).ConfigureAwait(false);

            return Ok(new
            {
                file.Id,
                file.Name,
                file.FileName,
                file.Language,
                file.RecognitionState,
                file.DateCreated,
                file.DateProcessed
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

            if (!SupportedLanguages.IsSupported(createFileModel.Language))
                return StatusCode(406);

            var userId = Guid.Parse(HttpContext.User.Identity.Name);

            var fileToUpload = files[0];
            if (!fileToUpload.IsSupportedType())
                return StatusCode(415);

            var fileItem = new FileItem
            {
                Id = Guid.NewGuid(),
                UserId = Guid.Parse(HttpContext.User.Identity.Name),
                Name = createFileModel.Name,
                FileName = fileToUpload.Name,
                Language = createFileModel.Language,
                DateCreated = DateTime.UtcNow
            };

            var source = await fileToUpload.GetBytesAsync().ConfigureAwait(false);
            var audioSource = new AudioSource
            {
                Id = Guid.NewGuid(),
                FileItemId = fileItem.Id,
                OriginalSource = source,
                ContentType = fileToUpload.ContentType
            };

            await _fileItemService.AddAsync(fileItem).ConfigureAwait(false);
            await _audioSourceService.AddAsync(audioSource).ConfigureAwait(false);

            BackgroundJob.Enqueue(() => _wavFileManager.RunConversionToWav(audioSource, userId));

            return Ok();
        }

        [HttpPut("/api/files/update")]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> Update([FromForm] UploadFileModel uploadFileModel)
        {
            var files = Request.Form.Files;
            if (files.Count > 1)
                return StatusCode(416);

            if (!SupportedLanguages.IsSupported(uploadFileModel.Language))
                return StatusCode(406);

            var userId = Guid.Parse(HttpContext.User.Identity.Name);

            var fileItem = new FileItem
            {
                Id = uploadFileModel.FileItemId,
                UserId = Guid.Parse(HttpContext.User.Identity.Name),
                Name = uploadFileModel.Name,
                Language = uploadFileModel.Language
            };

            if (files.Any())
            {
                var fileToUpoad = files.First();
                fileItem.FileName = fileToUpoad.Name;

                var source = await fileToUpoad.GetBytesAsync().ConfigureAwait(false);
                var audioSource = new AudioSource
                {
                    FileItemId = fileItem.Id,
                    OriginalSource = source,
                    WavSource = null,
                    ContentType = fileToUpoad.ContentType,
                    TotalTime = default(TimeSpan)
                };

                await _audioSourceService.UpdateAsync(audioSource).ConfigureAwait(false);

                BackgroundJob.Enqueue(() => _wavFileManager.RunConversionToWav(audioSource, userId));
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
            var fileItem = await _fileItemService.GetAsync(userId, transcribeFileItemModel.FileItemId).ConfigureAwait(false);

            if (fileItem.RecognitionState != RecognitionState.Prepared)
                return BadRequest();

            var canRunRecognition = await _speechRecognitionManager.CanRunRecognition(fileItem, userId).ConfigureAwait(false);
            if (!canRunRecognition)
                return StatusCode(403);

            BackgroundJob.Enqueue(() => _speechRecognitionManager.RunRecognition(fileItem, userId));

            return Ok();
        }
    }
}