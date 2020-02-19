using System;
using System.Collections.Generic;
using System.IO;
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
using RewriteMe.Domain.Dtos;
using RewriteMe.Domain.Enums;
using RewriteMe.Domain.Extensions;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.WebApi.Extensions;
using Swashbuckle.AspNetCore.Annotations;
using IOFile = System.IO.File;

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
        private readonly IFileAccessService _fileAccessService;
        private readonly IApplicationLogService _applicationLogService;

        public UploadedChunkController(
            IFileItemService fileItemService,
            IFileItemSourceService fileItemSourceService,
            IUploadedChunkService uploadedChunkService,
            IInternalValueService internalValueService,
            IFileAccessService fileAccessService,
            IApplicationLogService applicationLogService)
        {
            _fileItemService = fileItemService;
            _fileItemSourceService = fileItemSourceService;
            _uploadedChunkService = uploadedChunkService;
            _internalValueService = internalValueService;
            _fileAccessService = fileAccessService;
            _applicationLogService = applicationLogService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(StorageConfiguration), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(OperationId = "GetChunksStorageConfiguration")]
        public async Task<IActionResult> GetChunksStorageConfiguration()
        {
            try
            {
                var storageSetting = await _internalValueService.GetValueAsync(InternalValues.ChunksStorageSetting).ConfigureAwait(false);
                var configuration = new StorageConfiguration
                {
                    StorageSetting = storageSetting
                };

                return Ok(configuration);
            }
            catch (Exception ex)
            {
                await _applicationLogService.ErrorAsync($"{ExceptionFormatter.FormatException(ex)}").ConfigureAwait(false);
            }

            return StatusCode((int)HttpStatusCode.InternalServerError);
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(typeof(OkDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorCode), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(OperationId = "UploadChunkFile")]
        [RequestFormLimits(ValueLengthLimit = int.MaxValue, MultipartBodyLengthLimit = int.MaxValue)]
        [RequestSizeLimit(int.MaxValue)]
        public async Task<IActionResult> Upload(Guid fileItemId, int order, StorageSetting storageSetting, Guid applicationId, IFormFile file, CancellationToken cancellationToken)
        {
            try
            {
                if (file == null)
                    return BadRequest(ErrorCode.EC100);

                var uploadedFileSource = await file.GetBytesAsync(cancellationToken).ConfigureAwait(false);
                cancellationToken.ThrowIfCancellationRequested();

                await _uploadedChunkService.SaveAsync(fileItemId, order, storageSetting, applicationId, uploadedFileSource, cancellationToken).ConfigureAwait(false);

                return Ok(new OkDto());
            }
            catch (OperationCanceledException)
            {
                return BadRequest(ErrorCode.EC800);
            }
            catch (Exception ex)
            {
                await _applicationLogService.ErrorAsync($"{ExceptionFormatter.FormatException(ex)}").ConfigureAwait(false);
            }

            return StatusCode((int)HttpStatusCode.InternalServerError);
        }

        [HttpPut("submit")]
        [ProducesResponseType(typeof(FileItemDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorCode), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(OperationId = "SubmitChunks")]
        public async Task<IActionResult> Submit(Guid fileItemId, int chunksCount, StorageSetting chunksStorageSetting, Guid applicationId, CancellationToken cancellationToken)
        {
            try
            {
                var userId = HttpContext.User.GetNameIdentifier();
                var fileItem = await _fileItemService.GetAsync(userId, fileItemId).ConfigureAwait(false);
                if (fileItem == null)
                    return BadRequest(ErrorCode.EC101);

                cancellationToken.ThrowIfCancellationRequested();

                var chunkList = await _uploadedChunkService.GetAllAsync(fileItemId, applicationId, cancellationToken).ConfigureAwait(false);
                var chunks = chunkList.ToList();
                cancellationToken.ThrowIfCancellationRequested();

                if (chunks.Count != chunksCount)
                    return BadRequest(ErrorCode.EC102);

                var uploadedFileSource = new List<byte>();
                var chunksFileItemStoragePath = _fileAccessService.GetChunksFileItemStoragePath(fileItemId);
                foreach (var chunk in chunks.OrderBy(x => x.Order))
                {
                    byte[] bytes;
                    if (chunksStorageSetting == StorageSetting.Database)
                    {
                        bytes = chunk.Source;
                    }
                    else
                    {
                        var filePath = Path.Combine(chunksFileItemStoragePath, chunk.Id.ToString());
                        bytes = await IOFile.ReadAllBytesAsync(filePath, cancellationToken).ConfigureAwait(false);
                    }

                    uploadedFileSource.AddRange(bytes);
                }

                cancellationToken.ThrowIfCancellationRequested();
                var uploadedFile = await _fileItemService.UploadFileToStorageAsync(userId, fileItemId, uploadedFileSource.ToArray()).ConfigureAwait(false);

                var totalTime = _fileItemService.GetAudioTotalTime(uploadedFile.FilePath);
                if (!totalTime.HasValue)
                {
                    _fileItemService.CleanUploadedData(uploadedFile.DirectoryPath);

                    return BadRequest(ErrorCode.EC201);
                }

                await _fileItemService.UpdateUploadStatus(fileItemId, UploadStatus.InProgress, applicationId).ConfigureAwait(false);

                var dateUpdated = DateTime.UtcNow;
                var storageSetting = await _internalValueService.GetValueAsync(InternalValues.StorageSetting).ConfigureAwait(false);

                fileItem.ApplicationId = applicationId;
                fileItem.OriginalSourceFileName = uploadedFile.FileName;
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

                    return BadRequest(ErrorCode.EC400);
                }
                finally
                {
                    await _uploadedChunkService.DeleteAsync(fileItemId, applicationId).ConfigureAwait(false);
                }

                return Ok(fileItem.ToDto());
            }
            catch (OperationCanceledException)
            {
                return BadRequest(ErrorCode.EC800);
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