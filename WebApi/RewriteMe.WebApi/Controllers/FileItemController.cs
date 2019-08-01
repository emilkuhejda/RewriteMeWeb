﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RewriteMe.Common.Utils;
using RewriteMe.Domain;
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
        private readonly IApplicationLogService _applicationLogService;
        private readonly ISpeechRecognitionManager _speechRecognitionManager;
        private readonly IWavFileManager _wavFileManager;

        public FileItemController(
            IFileItemService fileItemService,
            IApplicationLogService applicationLogService,
            ISpeechRecognitionManager speechRecognitionManager,
            IWavFileManager wavFileManager)
        {
            _fileItemService = fileItemService;
            _applicationLogService = applicationLogService;
            _speechRecognitionManager = speechRecognitionManager;
            _wavFileManager = wavFileManager;
        }

        [HttpGet("/api/files")]
        [ProducesResponseType(typeof(IEnumerable<FileItemDto>), StatusCodes.Status200OK)]
        [SwaggerOperation(OperationId = "GetFileItems")]
        public async Task<IActionResult> Get(DateTime updatedAfter, Guid applicationId)
        {
            var userId = HttpContext.User.GetNameIdentifier();
            var files = await _fileItemService.GetAllAsync(userId, updatedAfter.ToUniversalTime(), applicationId).ConfigureAwait(false);

            return Ok(files.Select(x => x.ToDto()));
        }

        [HttpGet("/api/files/deleted")]
        [ProducesResponseType(typeof(IEnumerable<Guid>), StatusCodes.Status200OK)]
        [SwaggerOperation(OperationId = "GetDeletedFileItemIds")]
        public async Task<IActionResult> GetDeletedFileItemIds(DateTime updatedAfter, Guid applicationId)
        {
            var userId = HttpContext.User.GetNameIdentifier();
            var ids = await _fileItemService.GetAllDeletedIdsAsync(userId, updatedAfter.ToUniversalTime(), applicationId).ConfigureAwait(false);

            return Ok(ids);
        }

        [HttpGet("/api/files/deleted-total-time")]
        [ProducesResponseType(typeof(TimeSpan), StatusCodes.Status200OK)]
        [SwaggerOperation(OperationId = "GetDeletedFileItemsTotalTime")]
        public async Task<IActionResult> GetDeletedTotalTime()
        {
            var userId = HttpContext.User.GetNameIdentifier();
            var totalTime = await _fileItemService.GetDeletedFileItemsTotalTime(userId).ConfigureAwait(false);

            return Ok(totalTime);
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

        [HttpPost("/api/files/upload")]
        [ProducesResponseType(typeof(FileItemDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
        [SwaggerOperation(OperationId = "UploadFileItem")]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> Upload(string name, string language, string fileName, Guid applicationId, [FromForm]IFormFile file)
        {
            if (file == null)
                return BadRequest();

            if (!string.IsNullOrWhiteSpace(language) && !SupportedLanguages.IsSupported(language))
                return StatusCode(406);

            var fileItemId = Guid.NewGuid();
            var uploadedFileSource = await file.GetBytesAsync().ConfigureAwait(false);
            var uploadedFile = await _fileItemService.UploadFileAsync(fileItemId, uploadedFileSource).ConfigureAwait(false);

            TimeSpan totalTime;
            try
            {
                totalTime = _fileItemService.GetAudioTotalTime(uploadedFile.FilePath);
            }
            catch (Exception)
            {
                Directory.Delete(uploadedFile.DirectoryPath, true);

                return StatusCode(415);
            }

            var userId = HttpContext.User.GetNameIdentifier();
            var dateCreated = DateTime.UtcNow;
            var fileItem = new FileItem
            {
                Id = fileItemId,
                UserId = userId,
                ApplicationId = applicationId,
                Name = name,
                FileName = fileName,
                Language = language,
                OriginalSourceFileName = uploadedFile.FileName,
                OriginalContentType = file.ContentType,
                TotalTime = totalTime,
                DateCreated = dateCreated,
                DateUpdated = dateCreated
            };

            try
            {
                await _fileItemService.AddAsync(fileItem).ConfigureAwait(false);
            }
            catch (DbUpdateException ex)
            {
                Directory.Delete(uploadedFile.DirectoryPath, true);

                await _applicationLogService.ErrorAsync(ExceptionFormatter.FormatException(ex), userId).ConfigureAwait(false);

                return BadRequest();
            }

            BackgroundJob.Enqueue(() => _wavFileManager.RunConversionToWav(fileItem, userId));

            return Ok(fileItem.ToDto());
        }

        [HttpPut("/api/files/update")]
        [ProducesResponseType(typeof(FileItemDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
        [SwaggerOperation(OperationId = "UpdateFileItem")]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> Update([FromForm]UpdateFileItemModel updateFileItemModel)
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
                DateUpdated = DateTime.UtcNow
            };

            await _fileItemService.UpdateAsync(fileItem).ConfigureAwait(false);

            return Ok(new OkDto());
        }

        [HttpDelete("/api/files/delete")]
        [ProducesResponseType(typeof(TimeSpan), StatusCodes.Status200OK)]
        [SwaggerOperation(OperationId = "DeleteFileItem")]
        public async Task<IActionResult> Delete(Guid fileItemId, Guid applicationId)
        {
            var userId = HttpContext.User.GetNameIdentifier();

            await _fileItemService.DeleteAsync(userId, fileItemId, applicationId).ConfigureAwait(false);
            var totalTime = await _fileItemService.GetDeletedFileItemsTotalTime(userId).ConfigureAwait(false);

            return Ok(totalTime);
        }

        [HttpDelete("/api/files/delete-all")]
        [ProducesResponseType(typeof(OkDto), StatusCodes.Status200OK)]
        [SwaggerOperation(OperationId = "DeleteAllFileItems")]
        public async Task<IActionResult> DeleteAll(IEnumerable<DeletedFileItemModel> fileItems, Guid applicationId)
        {
            var userId = HttpContext.User.GetNameIdentifier();

            await _fileItemService.DeleteAllAsync(userId, fileItems.Select(x => x.ToDeletedFileItem()), applicationId).ConfigureAwait(false);

            return Ok(new OkDto());
        }

        [HttpPut("/api/files/transcribe")]
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