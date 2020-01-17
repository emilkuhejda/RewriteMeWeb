using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RewriteMe.Business.Configuration;
using RewriteMe.Common.Utils;
using RewriteMe.Domain.Enums;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.Domain.Transcription;
using RewriteMe.WebApi.Dtos;
using RewriteMe.WebApi.Extensions;
using Swashbuckle.AspNetCore.Annotations;

namespace RewriteMe.WebApi.Controllers.V1
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/chunks")]
    [Produces("application/json")]
    [Authorize(Roles = nameof(Role.User))]
    [ApiController]
    public class UploadedChunkController : ControllerBase
    {
        private readonly IFileItemService _fileItemService;
        private readonly IFileItemSourceService _fileItemSourceService;
        private readonly IUploadedChunkService _uploadedChunkService;
        private readonly IInternalValueService _internalValueService;
        private readonly IApplicationLogService _applicationLogService;

        public UploadedChunkController(
            IFileItemService fileItemService,
            IFileItemSourceService fileItemSourceService,
            IUploadedChunkService uploadedChunkService,
            IInternalValueService internalValueService,
            IApplicationLogService applicationLogService)
        {
            _fileItemService = fileItemService;
            _fileItemSourceService = fileItemSourceService;
            _uploadedChunkService = uploadedChunkService;
            _internalValueService = internalValueService;
            _applicationLogService = applicationLogService;
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(typeof(OkDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(OperationId = "UploadChunkFile")]
        [RequestFormLimits(ValueLengthLimit = int.MaxValue, MultipartBodyLengthLimit = int.MaxValue)]
        [RequestSizeLimit(int.MaxValue)]
        public async Task<IActionResult> Upload(Guid fileItemId, int order, Guid applicationId, IFormFile file, CancellationToken cancellationToken)
        {
            try
            {
                if (file == null)
                    return NotFound();

                var uploadedFileSource = await file.GetBytesAsync(cancellationToken).ConfigureAwait(false);
                cancellationToken.ThrowIfCancellationRequested();

                var uploadedChunk = new UploadedChunk
                {
                    Id = Guid.NewGuid(),
                    FileItemId = fileItemId,
                    ApplicationId = applicationId,
                    Source = uploadedFileSource,
                    Order = order,
                    DateCreatedUtc = DateTime.UtcNow
                };

                await _uploadedChunkService.AddAsync(uploadedChunk).ConfigureAwait(false);

                return Ok(new OkDto());
            }
            catch (OperationCanceledException)
            {
                return StatusCode((int)HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                await _applicationLogService.ErrorAsync($"{ExceptionFormatter.FormatException(ex)}").ConfigureAwait(false);
            }

            return StatusCode((int)HttpStatusCode.InternalServerError);
        }

        [HttpPut("submit")]
        [ProducesResponseType(typeof(FileItemDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status405MethodNotAllowed)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
        [SwaggerOperation(OperationId = "SubmitChunks")]
        public async Task<IActionResult> Submit(Guid fileItemId, int chunksCount, Guid applicationId, CancellationToken cancellationToken)
        {
            try
            {
                var userId = HttpContext.User.GetNameIdentifier();
                var fileItem = await _fileItemService.GetAsync(userId, fileItemId).ConfigureAwait(false);
                if (fileItem == null)
                    return NotFound();

                cancellationToken.ThrowIfCancellationRequested();

                var chunkList = await _uploadedChunkService.GetAllAsync(fileItemId, applicationId, cancellationToken).ConfigureAwait(false);
                var chunks = chunkList.ToList();
                cancellationToken.ThrowIfCancellationRequested();

                if (chunks.Count != chunksCount)
                    return StatusCode((int)HttpStatusCode.MethodNotAllowed);

                var uploadedFileSource = chunks.OrderBy(x => x.Order).SelectMany(x => x.Source).ToArray();
                var uploadedFile = await _fileItemService.UploadFileToStorageAsync(fileItemId, uploadedFileSource).ConfigureAwait(false);

                var totalTime = _fileItemService.GetAudioTotalTime(uploadedFile.FilePath);
                if (!totalTime.HasValue)
                {
                    _fileItemService.CleanUploadedData(uploadedFile.DirectoryPath);

                    return StatusCode((int)HttpStatusCode.UnsupportedMediaType);
                }

                await _fileItemService.UpdateUploadStatus(fileItemId, UploadStatus.InProgress, applicationId).ConfigureAwait(false);

                var dateUpdated = DateTime.UtcNow;
                var storageSetting = await _internalValueService.GetValueAsync(InternalValues.StorageSetting).ConfigureAwait(false);

                fileItem.ApplicationId = applicationId;
                fileItem.OriginalSourceFileName = uploadedFile.FileName;
                //fileItem.OriginalContentType = contentType;
                fileItem.Storage = storageSetting;
                fileItem.TotalTime = totalTime.Value;
                fileItem.DateUpdatedUtc = dateUpdated;

                try
                {
                    await _fileItemService.UpdateAsync(fileItem).ConfigureAwait(false);

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
            catch (OperationCanceledException)
            {
                return StatusCode((int)HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                await _applicationLogService.ErrorAsync($"{ExceptionFormatter.FormatException(ex)}").ConfigureAwait(false);
            }

            return StatusCode((int)HttpStatusCode.InternalServerError);
        }

        [HttpDelete("{fileItemId}")]
        [ProducesResponseType(typeof(OkDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(OperationId = "DeleteChunks")]
        public async Task<IActionResult> DeleteChunks(Guid fileItemId, Guid applicationId)
        {
            try
            {
                await _uploadedChunkService.DeleteAsync(fileItemId, applicationId).ConfigureAwait(false);

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