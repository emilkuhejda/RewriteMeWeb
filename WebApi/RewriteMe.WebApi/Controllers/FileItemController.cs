using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RewriteMe.Domain;
using RewriteMe.Domain.Enums;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.Domain.Managers;
using RewriteMe.Domain.Transcription;
using RewriteMe.WebApi.Dtos;
using RewriteMe.WebApi.Extensions;
using RewriteMe.WebApi.Models;
using Swashbuckle.AspNetCore.Annotations;

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
        [ProducesResponseType(typeof(IEnumerable<FileItemDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(OperationId = "GetFileItems")]
        public async Task<IActionResult> Get(int minimumVersion = 0)
        {
            var userId = HttpContext.User.GetNameIdentifier();
            var files = await _fileItemService.GetAllAsync(userId, minimumVersion).ConfigureAwait(false);

            return Ok(files.Select(x => new FileItemDto
            {
                Id = x.Id,
                Name = x.Name,
                FileName = x.FileName,
                Language = x.Language,
                RecognitionState = x.RecognitionState.ToString(),
                DateCreated = x.DateCreated,
                DateProcessed = x.DateProcessed,
                Version = x.Version
            }));
        }

        [HttpGet("/api/files/{fileItemId}")]
        [ProducesResponseType(typeof(FileItemDto), StatusCodes.Status200OK)]
        [SwaggerOperation(OperationId = "GetFileItem")]
        public async Task<IActionResult> Get(Guid fileItemId)
        {
            var userId = HttpContext.User.GetNameIdentifier();
            var file = await _fileItemService.GetAsync(userId, fileItemId).ConfigureAwait(false);

            return Ok(new FileItemDto
            {
                Id = file.Id,
                Name = file.Name,
                FileName = file.FileName,
                Language = file.Language,
                RecognitionState = file.RecognitionState.ToString(),
                DateCreated = file.DateCreated,
                DateProcessed = file.DateProcessed,
                Version = file.Version
            });
        }

        [HttpPost("/api/files/create")]
        [ProducesResponseType(typeof(OkDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
        [ProducesResponseType(StatusCodes.Status416RangeNotSatisfiable)]
        [SwaggerOperation(OperationId = "CreateFileItem")]
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

            var userId = HttpContext.User.GetNameIdentifier();

            var fileToUpload = files[0];
            if (!fileToUpload.IsSupportedType())
                return StatusCode(415);

            var fileItem = new FileItem
            {
                Id = Guid.NewGuid(),
                UserId = HttpContext.User.GetNameIdentifier(),
                Name = createFileModel.Name,
                FileName = fileToUpload.Name,
                Language = createFileModel.Language,
                DateCreated = DateTime.UtcNow,
                Version = 0
            };

            var source = await fileToUpload.GetBytesAsync().ConfigureAwait(false);
            var audioSource = new AudioSource
            {
                Id = Guid.NewGuid(),
                FileItemId = fileItem.Id,
                OriginalSource = source,
                ContentType = fileToUpload.ContentType,
                Version = 0
            };

            await _fileItemService.AddAsync(fileItem).ConfigureAwait(false);
            await _audioSourceService.AddAsync(audioSource).ConfigureAwait(false);

            BackgroundJob.Enqueue(() => _wavFileManager.RunConversionToWav(audioSource, userId));

            return Ok(new OkDto());
        }

        [HttpPut("/api/files/update")]
        [ProducesResponseType(typeof(OkDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(StatusCodes.Status416RangeNotSatisfiable)]
        [SwaggerOperation(OperationId = "UpdateFileItem")]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> Update([FromForm] UploadFileModel uploadFileModel)
        {
            var files = Request.Form.Files;
            if (files.Count > 1)
                return StatusCode(416);

            if (!SupportedLanguages.IsSupported(uploadFileModel.Language))
                return StatusCode(406);

            var userId = HttpContext.User.GetNameIdentifier();

            var fileItem = new FileItem
            {
                Id = uploadFileModel.FileItemId,
                UserId = HttpContext.User.GetNameIdentifier(),
                Name = uploadFileModel.Name,
                Language = uploadFileModel.Language,
                Version = uploadFileModel.FileItemVersion
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
                    TotalTime = default(TimeSpan),
                    Version = uploadFileModel.SourceVersion
                };

                await _audioSourceService.UpdateAsync(audioSource).ConfigureAwait(false);

                BackgroundJob.Enqueue(() => _wavFileManager.RunConversionToWav(audioSource, userId));
            }

            await _fileItemService.UpdateAsync(fileItem).ConfigureAwait(false);

            return Ok(new OkDto());
        }

        [HttpDelete("/api/files/{id}")]
        [ProducesResponseType(typeof(OkDto), StatusCodes.Status200OK)]
        [SwaggerOperation(OperationId = "RemoveFileItem")]
        public async Task<IActionResult> Remove([FromRoute] string id)
        {
            var userId = HttpContext.User.GetNameIdentifier();
            var fileItemId = Guid.Parse(id);

            await _fileItemService.RemoveAsync(userId, fileItemId).ConfigureAwait(false);

            return Ok(new OkDto());
        }

        [HttpPost("/api/files/transcribe")]
        [ProducesResponseType(typeof(OkDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [SwaggerOperation(OperationId = "TranscribeFileItem")]
        public async Task<IActionResult> Transcribe([FromBody] TranscribeFileItemModel transcribeFileItemModel)
        {
            var userId = HttpContext.User.GetNameIdentifier();
            var fileItem = await _fileItemService.GetAsync(userId, transcribeFileItemModel.FileItemId).ConfigureAwait(false);

            if (fileItem.RecognitionState != RecognitionState.Prepared)
                return BadRequest();

            var canRunRecognition = await _speechRecognitionManager.CanRunRecognition(fileItem, userId).ConfigureAwait(false);
            if (!canRunRecognition)
                return StatusCode(403);

            BackgroundJob.Enqueue(() => _speechRecognitionManager.RunRecognition(fileItem, userId));

            return Ok(new OkDto());
        }
    }
}