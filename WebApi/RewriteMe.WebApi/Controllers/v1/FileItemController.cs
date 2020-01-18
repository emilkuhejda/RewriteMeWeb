using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RewriteMe.Business.Configuration;
using RewriteMe.Common.Utils;
using RewriteMe.Domain;
using RewriteMe.Domain.Enums;
using RewriteMe.Domain.Interfaces.Managers;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.Domain.Transcription;
using RewriteMe.WebApi.Dtos;
using RewriteMe.WebApi.Extensions;
using RewriteMe.WebApi.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace RewriteMe.WebApi.Controllers.V1
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/files")]
    [Produces("application/json")]
    [Authorize(Roles = nameof(Role.User))]
    [ApiController]
    public class FileItemController : ControllerBase
    {
        private readonly IFileItemService _fileItemService;
        private readonly IFileItemSourceService _fileItemSourceService;
        private readonly IInternalValueService _internalValueService;
        private readonly IApplicationLogService _applicationLogService;
        private readonly ISpeechRecognitionManager _speechRecognitionManager;

        public FileItemController(
            IFileItemService fileItemService,
            IFileItemSourceService fileItemSourceService,
            IInternalValueService internalValueService,
            IApplicationLogService applicationLogService,
            ISpeechRecognitionManager speechRecognitionManager)
        {
            _fileItemService = fileItemService;
            _fileItemSourceService = fileItemSourceService;
            _internalValueService = internalValueService;
            _applicationLogService = applicationLogService;
            _speechRecognitionManager = speechRecognitionManager;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<FileItemDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(OperationId = "GetFileItems")]
        public async Task<IActionResult> Get(DateTime updatedAfter, Guid applicationId)
        {
            try
            {
                var userId = HttpContext.User.GetNameIdentifier();
                var files = await _fileItemService.GetAllAsync(userId, updatedAfter.ToUniversalTime(), applicationId).ConfigureAwait(false);

                return Ok(files.Select(x => x.ToDto()));
            }
            catch (Exception ex)
            {
                await _applicationLogService.ErrorAsync($"{ExceptionFormatter.FormatException(ex)}").ConfigureAwait(false);
            }

            return StatusCode((int)HttpStatusCode.InternalServerError);
        }

        [HttpGet("deleted")]
        [ProducesResponseType(typeof(IEnumerable<Guid>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(OperationId = "GetDeletedFileItemIds")]
        public async Task<IActionResult> GetDeletedFileItemIds(DateTime updatedAfter, Guid applicationId)
        {
            try
            {
                var userId = HttpContext.User.GetNameIdentifier();
                var ids = await _fileItemService.GetAllDeletedIdsAsync(userId, updatedAfter.ToUniversalTime(), applicationId).ConfigureAwait(false);

                return Ok(ids);
            }
            catch (Exception ex)
            {
                await _applicationLogService.ErrorAsync($"{ExceptionFormatter.FormatException(ex)}").ConfigureAwait(false);
            }

            return StatusCode((int)HttpStatusCode.InternalServerError);
        }

        [HttpGet("temporary-deleted")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> GetTemporaryDeletedFileItems()
        {
            try
            {
                var userId = HttpContext.User.GetNameIdentifier();
                var fileItems = await _fileItemService.GetTemporaryDeletedFileItemsAsync(userId).ConfigureAwait(false);

                return Ok(fileItems);
            }
            catch (Exception ex)
            {
                await _applicationLogService.ErrorAsync($"{ExceptionFormatter.FormatException(ex)}").ConfigureAwait(false);
            }

            return StatusCode((int)HttpStatusCode.InternalServerError);
        }

        [HttpGet("{fileItemId}")]
        [ProducesResponseType(typeof(FileItemDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(OperationId = "GetFileItem")]
        public async Task<IActionResult> Get(Guid fileItemId)
        {
            try
            {
                var userId = HttpContext.User.GetNameIdentifier();
                var file = await _fileItemService.GetAsync(userId, fileItemId).ConfigureAwait(false);

                return Ok(file.ToDto());
            }
            catch (Exception ex)
            {
                await _applicationLogService.ErrorAsync($"{ExceptionFormatter.FormatException(ex)}").ConfigureAwait(false);
            }

            return StatusCode((int)HttpStatusCode.InternalServerError);
        }

        [HttpPost("create")]
        [ProducesResponseType(typeof(FileItemDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(OperationId = "CreateFileItem")]
        public async Task<IActionResult> CreateFileItem(string name, string language, string fileName, DateTime dateCreated, Guid applicationId)
        {
            try
            {
                var userId = HttpContext.User.GetNameIdentifier();
                if (!string.IsNullOrWhiteSpace(language) && !SupportedLanguages.IsSupported(language))
                    return StatusCode(406);

                var fileItemId = Guid.NewGuid();
                var dateUpdated = DateTime.UtcNow;
                var storageSetting = await _internalValueService.GetValueAsync(InternalValues.StorageSetting).ConfigureAwait(false);
                var fileItem = new FileItem
                {
                    Id = fileItemId,
                    UserId = userId,
                    ApplicationId = applicationId,
                    Name = name,
                    FileName = fileName,
                    Language = language,
                    Storage = storageSetting,
                    DateCreated = dateCreated,
                    DateUpdatedUtc = dateUpdated
                };

                await _fileItemService.AddAsync(fileItem).ConfigureAwait(false);

                return Ok(fileItem.ToDto());
            }
            catch (Exception ex)
            {
                await _applicationLogService.ErrorAsync($"{ExceptionFormatter.FormatException(ex)}").ConfigureAwait(false);
            }

            return StatusCode((int)HttpStatusCode.InternalServerError);
        }

        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(typeof(FileItemDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(OperationId = "UploadFileItem")]
        [RequestFormLimits(ValueLengthLimit = int.MaxValue, MultipartBodyLengthLimit = int.MaxValue)]
        [RequestSizeLimit(int.MaxValue)]
        public async Task<IActionResult> Upload(string name, string language, string fileName, DateTime dateCreated, Guid applicationId, IFormFile file)
        {
            try
            {
                if (file == null)
                    return BadRequest();

                if (!string.IsNullOrWhiteSpace(language) && !SupportedLanguages.IsSupported(language))
                    return StatusCode(406);

                var fileItemId = Guid.NewGuid();
                var uploadedFileSource = await file.GetBytesAsync().ConfigureAwait(false);
                var uploadedFile = await _fileItemService.UploadFileToStorageAsync(fileItemId, uploadedFileSource).ConfigureAwait(false);

                var totalTime = _fileItemService.GetAudioTotalTime(uploadedFile.FilePath);
                if (!totalTime.HasValue)
                {
                    _fileItemService.CleanUploadedData(uploadedFile.DirectoryPath);

                    return StatusCode(415);
                }

                var dateUpdated = DateTime.UtcNow;
                var storageSetting = await _internalValueService.GetValueAsync(InternalValues.StorageSetting).ConfigureAwait(false);
                var userId = HttpContext.User.GetNameIdentifier();
                var fileItem = new FileItem
                {
                    Id = fileItemId,
                    UserId = userId,
                    ApplicationId = applicationId,
                    Name = name,
                    FileName = fileName,
                    Language = language,
                    OriginalSourceFileName = uploadedFile.FileName,
                    Storage = storageSetting,
                    TotalTime = totalTime.Value,
                    DateCreated = dateCreated,
                    DateUpdatedUtc = dateUpdated
                };

                try
                {
                    await _fileItemService.AddAsync(fileItem).ConfigureAwait(false);
                    await _fileItemService.UpdateUploadStatus(fileItem.Id, UploadStatus.InProgress, applicationId).ConfigureAwait(false);

                    if (storageSetting == StorageSetting.Database ||
                        await _internalValueService.GetValueAsync(InternalValues.IsDatabaseBackupEnabled).ConfigureAwait(false))
                    {
                        await _fileItemSourceService.AddFileItemSourceAsync(fileItem, uploadedFile.FilePath).ConfigureAwait(false);
                    }

                    await _fileItemService.UpdateUploadStatus(fileItem.Id, UploadStatus.Completed, applicationId).ConfigureAwait(false);

                    if (storageSetting == StorageSetting.Database)
                    {
                        _fileItemService.CleanUploadedData(uploadedFile.DirectoryPath);
                    }
                }
                catch (DbUpdateException ex)
                {
                    _fileItemService.CleanUploadedData(uploadedFile.DirectoryPath);

                    await _applicationLogService.ErrorAsync(ExceptionFormatter.FormatException(ex), userId).ConfigureAwait(false);

                    return Conflict();
                }

                return Ok(fileItem.ToDto());
            }
            catch (Exception ex)
            {
                await _applicationLogService.ErrorAsync($"{ExceptionFormatter.FormatException(ex)}").ConfigureAwait(false);
            }

            return StatusCode((int)HttpStatusCode.InternalServerError);
        }

        [HttpPut("update")]
        [ProducesResponseType(typeof(FileItemDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(OperationId = "UpdateFileItem")]
        public async Task<IActionResult> Update([FromForm]UpdateFileItemModel updateFileItemModel)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(updateFileItemModel.Language) || !SupportedLanguages.IsSupported(updateFileItemModel.Language))
                    return StatusCode(406);

                var userId = HttpContext.User.GetNameIdentifier();
                var fileItem = new FileItem
                {
                    Id = updateFileItemModel.FileItemId,
                    UserId = userId,
                    ApplicationId = updateFileItemModel.ApplicationId,
                    Name = updateFileItemModel.Name,
                    Language = updateFileItemModel.Language,
                    DateUpdatedUtc = DateTime.UtcNow
                };

                await _fileItemService.UpdateAsync(fileItem).ConfigureAwait(false);

                return Ok(new OkDto());
            }
            catch (Exception ex)
            {
                await _applicationLogService.ErrorAsync($"{ExceptionFormatter.FormatException(ex)}").ConfigureAwait(false);
            }

            return StatusCode((int)HttpStatusCode.InternalServerError);
        }

        [HttpDelete("delete")]
        [ProducesResponseType(typeof(OkDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(OperationId = "DeleteFileItem")]
        public async Task<IActionResult> Delete(Guid fileItemId, Guid applicationId)
        {
            try
            {
                var userId = HttpContext.User.GetNameIdentifier();
                await _fileItemService.DeleteAsync(userId, fileItemId, applicationId).ConfigureAwait(false);

                return Ok(new OkDto());
            }
            catch (Exception ex)
            {
                await _applicationLogService.ErrorAsync($"{ExceptionFormatter.FormatException(ex)}").ConfigureAwait(false);
            }

            return StatusCode((int)HttpStatusCode.InternalServerError);
        }

        [HttpDelete("delete-all")]
        [ProducesResponseType(typeof(OkDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(OperationId = "DeleteAllFileItems")]
        public async Task<IActionResult> DeleteAll(IEnumerable<DeletedFileItemModel> fileItems, Guid applicationId)
        {
            try
            {
                var userId = HttpContext.User.GetNameIdentifier();
                await _fileItemService.DeleteAllAsync(userId, fileItems.Select(x => x.ToDeletedFileItem()), applicationId).ConfigureAwait(false);

                return Ok(new OkDto());
            }
            catch (Exception ex)
            {
                await _applicationLogService.ErrorAsync($"{ExceptionFormatter.FormatException(ex)}").ConfigureAwait(false);
            }

            return StatusCode((int)HttpStatusCode.InternalServerError);
        }

        [HttpPut("permanent-delete-all")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> PermanentDeleteAll(IEnumerable<Guid> fileItemIds, Guid applicationId)
        {
            try
            {
                var userId = HttpContext.User.GetNameIdentifier();
                await _fileItemService.PermanentDeleteAllAsync(userId, fileItemIds, applicationId).ConfigureAwait(false);

                return Ok(new OkDto());
            }
            catch (Exception ex)
            {
                await _applicationLogService.ErrorAsync($"{ExceptionFormatter.FormatException(ex)}").ConfigureAwait(false);
            }

            return StatusCode((int)HttpStatusCode.InternalServerError);
        }

        [HttpPut("restore-all")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> RestoreAll(IEnumerable<Guid> fileItemIds, Guid applicationId)
        {
            try
            {
                var userId = HttpContext.User.GetNameIdentifier();
                await _fileItemService.RestoreAllAsync(userId, fileItemIds, applicationId).ConfigureAwait(false);

                return Ok(new OkDto());
            }
            catch (Exception ex)
            {
                await _applicationLogService.ErrorAsync($"{ExceptionFormatter.FormatException(ex)}").ConfigureAwait(false);
            }

            return StatusCode((int)HttpStatusCode.InternalServerError);
        }

        [HttpPut("transcribe")]
        [ProducesResponseType(typeof(OkDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status405MethodNotAllowed)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(OperationId = "TranscribeFileItem")]
        public async Task<IActionResult> Transcribe(Guid fileItemId, string language, Guid applicationId)
        {
            try
            {
                var userId = HttpContext.User.GetNameIdentifier();
                var fileItemExists = await _fileItemService.ExistsAsync(userId, fileItemId).ConfigureAwait(false);
                if (!fileItemExists)
                    return BadRequest();

                if (!SupportedLanguages.IsSupported(language))
                    return StatusCode(406);

                var canRunRecognition = await _speechRecognitionManager.CanRunRecognition(userId).ConfigureAwait(false);
                if (!canRunRecognition)
                    return StatusCode(405);

                await _fileItemService.UpdateLanguageAsync(fileItemId, language, applicationId).ConfigureAwait(false);

                BackgroundJob.Enqueue(() => _speechRecognitionManager.RunRecognition(userId, fileItemId));

                return Ok(new OkDto());
            }
            catch (Exception ex)
            {
                await _applicationLogService.ErrorAsync($"{ExceptionFormatter.FormatException(ex)}").ConfigureAwait(false);
            }

            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }
}