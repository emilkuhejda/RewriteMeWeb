using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RewriteMe.Domain;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.Domain.Managers;
using RewriteMe.Domain.Settings;
using RewriteMe.Domain.Transcription;
using RewriteMe.WebApi.Dtos;
using RewriteMe.WebApi.Extensions;
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
        public async Task<IActionResult> Get(DateTimeOffset updatedAfter, Guid applicationId)
        {
            var userId = HttpContext.User.GetNameIdentifier();
            var files = await _fileItemService.GetAllAsync(userId, updatedAfter.DateTime, applicationId).ConfigureAwait(false);

            return Ok(files.Select(x => x.ToDto()));
        }

        [HttpGet("/api/files/deleted")]
        [ProducesResponseType(typeof(IEnumerable<Guid>), StatusCodes.Status200OK)]
        [SwaggerOperation(OperationId = "GetDeletedFileItemIds")]
        public async Task<IActionResult> GetDeletedFileItemIds(DateTimeOffset updatedAfter, Guid applicationId)
        {
            var userId = HttpContext.User.GetNameIdentifier();
            var ids = await _fileItemService.GetAllDeletedIdsAsync(userId, updatedAfter.DateTime, applicationId).ConfigureAwait(false);

            return Ok(ids);
        }

        [HttpGet("/api/files/{fileItemId}")]
        [ProducesResponseType(typeof(FileItemDto), StatusCodes.Status200OK)]
        [SwaggerOperation(OperationId = "GetFileItem")]
        public async Task<IActionResult> Get(Guid fileItemId)
        {
            var userId = HttpContext.User.GetNameIdentifier();
            var file = await _fileItemService.GetAsync(userId, fileItemId).ConfigureAwait(false);

            return Ok(file.ToDto());
        }

        [HttpPost("/api/files/create")]
        [ProducesResponseType(typeof(FileItemDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
        [SwaggerOperation(OperationId = "UploadFileItem")]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> Create(string name, string language, string fileName, Guid applicationId, [FromForm]IFormFile file)
        {
            if (file == null)
                return BadRequest();

            if (!string.IsNullOrWhiteSpace(language) && !SupportedLanguages.IsSupported(language))
                return StatusCode(406);

            TimeSpan totalTime;
            try
            {
                totalTime = await file.GetTotalTime().ConfigureAwait(false);
            }
            catch (Exception)
            {
                return StatusCode(415);
            }

            var userId = HttpContext.User.GetNameIdentifier();

            var dateCreated = DateTime.UtcNow;
            var fileItem = new FileItem
            {
                Id = Guid.NewGuid(),
                UserId = HttpContext.User.GetNameIdentifier(),
                ApplicationId = applicationId,
                Name = name,
                FileName = fileName,
                Language = language,
                TotalTime = totalTime,
                DateCreated = dateCreated,
                DateUpdated = dateCreated,
                AudioSourceVersion = 1
            };

            var source = await file.GetBytesAsync().ConfigureAwait(false);
            var audioSource = new AudioSource
            {
                Id = Guid.NewGuid(),
                FileItemId = fileItem.Id,
                OriginalSource = source,
                ContentType = file.ContentType,
                Version = fileItem.AudioSourceVersion
            };

            await _fileItemService.AddAsync(fileItem).ConfigureAwait(false);
            await _audioSourceService.AddAsync(audioSource).ConfigureAwait(false);

            BackgroundJob.Enqueue(() => _wavFileManager.RunConversionToWav(audioSource, userId));

            var fileItemDto = fileItem.ToDto();
            fileItemDto.AudioSource = audioSource.ToDto();
            return Ok(fileItemDto);
        }

        [HttpPut("/api/files/update")]
        [ProducesResponseType(typeof(FileItemDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
        [SwaggerOperation(OperationId = "UpdateFileItem")]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> Update(Guid fileItemId, string name, string language, string fileName, Guid applicationId, [FromForm]IFormFile file = null)
        {
            if (!string.IsNullOrWhiteSpace(language) && !SupportedLanguages.IsSupported(language))
                return StatusCode(406);

            var userId = HttpContext.User.GetNameIdentifier();

            var fileItem = new FileItem
            {
                Id = fileItemId,
                UserId = HttpContext.User.GetNameIdentifier(),
                ApplicationId = applicationId,
                Name = name,
                Language = language,
                DateUpdated = DateTime.UtcNow
            };

            AudioSourceDto audioSourceDto = null;
            if (file != null)
            {
                TimeSpan totalTime;
                try
                {
                    totalTime = await file.GetTotalTime().ConfigureAwait(false);
                }
                catch (Exception)
                {
                    return StatusCode(415);
                }

                fileItem.AudioSourceVersion += 1;
                fileItem.FileName = fileName;
                fileItem.TotalTime = totalTime;

                var source = await file.GetBytesAsync().ConfigureAwait(false);
                var audioSource = new AudioSource
                {
                    FileItemId = fileItem.Id,
                    OriginalSource = source,
                    WavSource = null,
                    ContentType = file.ContentType,
                    TotalTime = default(TimeSpan),
                    Version = fileItem.AudioSourceVersion
                };

                await _audioSourceService.UpdateAsync(audioSource).ConfigureAwait(false);
                audioSourceDto = audioSource.ToDto();

                BackgroundJob.Enqueue(() => _wavFileManager.RunConversionToWav(audioSource, userId));
            }

            var fileItemDto = fileItem.ToDto();
            fileItemDto.AudioSource = audioSourceDto;

            await _fileItemService.UpdateAsync(fileItem).ConfigureAwait(false);

            return Ok(fileItemDto);
        }

        [HttpDelete("/api/files/delete")]
        [ProducesResponseType(typeof(OkDto), StatusCodes.Status200OK)]
        [SwaggerOperation(OperationId = "DeleteFileItem")]
        public async Task<IActionResult> Delete(Guid fileItemId, Guid applicationId)
        {
            var userId = HttpContext.User.GetNameIdentifier();

            await _fileItemService.DeleteAsync(userId, fileItemId, applicationId).ConfigureAwait(false);

            return Ok(new OkDto());
        }

        [HttpDelete("/api/files/delete-all")]
        [ProducesResponseType(typeof(OkDto), StatusCodes.Status200OK)]
        [SwaggerOperation(OperationId = "DeleteAllFileItem")]
        public async Task<IActionResult> DeleteAll(IEnumerable<DeletedFileItem> fileItems, Guid applicationId)
        {
            var userId = HttpContext.User.GetNameIdentifier();

            await _fileItemService.DeleteAllAsync(userId, fileItems, applicationId).ConfigureAwait(false);

            return Ok(new OkDto());
        }

        [HttpPost("/api/files/transcribe")]
        [ProducesResponseType(typeof(OkDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [SwaggerOperation(OperationId = "TranscribeFileItem")]
        public async Task<IActionResult> Transcribe(Guid fileItemId, string language, Guid applicationId)
        {
            var userId = HttpContext.User.GetNameIdentifier();

            var fileItemExists = await _fileItemService.ExistsAsync(userId, fileItemId).ConfigureAwait(false);
            if (!fileItemExists)
                return BadRequest();

            if (!SupportedLanguages.IsSupported(language))
                return StatusCode(406);

            var canRunRecognition = await _speechRecognitionManager.CanRunRecognition(userId, fileItemId).ConfigureAwait(false);
            if (!canRunRecognition)
                return StatusCode(403);

            await _fileItemService.UpdateLanguageAsync(fileItemId, language, applicationId).ConfigureAwait(false);

            BackgroundJob.Enqueue(() => _speechRecognitionManager.RunRecognition(userId, fileItemId));

            return Ok(new OkDto());
        }
    }
}