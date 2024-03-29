﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RewriteMe.Business.Configuration;
using RewriteMe.Common.Utils;
using RewriteMe.Domain.Dtos;
using RewriteMe.Domain.Enums;
using RewriteMe.Domain.Exceptions;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.WebApi.Commands;
using RewriteMe.WebApi.Extensions;
using Serilog;

namespace RewriteMe.WebApi.Handlers
{
    public class SubmitChunksHandler : IRequestHandler<SubmitChunksCommand, FileItemDto>
    {
        private readonly IFileItemService _fileItemService;
        private readonly IFileItemSourceService _fileItemSourceService;
        private readonly IUploadedChunkService _uploadedChunkService;
        private readonly IInternalValueService _internalValueService;
        private readonly IFileAccessService _fileAccessService;
        private readonly ILogger _logger;

        public SubmitChunksHandler(
            IFileItemService fileItemService,
            IFileItemSourceService fileItemSourceService,
            IUploadedChunkService uploadedChunkService,
            IInternalValueService internalValueService,
            IFileAccessService fileAccessService,
            ILogger logger)
        {
            _fileItemService = fileItemService;
            _fileItemSourceService = fileItemSourceService;
            _uploadedChunkService = uploadedChunkService;
            _internalValueService = internalValueService;
            _fileAccessService = fileAccessService;
            _logger = logger.ForContext<SubmitChunksHandler>();
        }

        public async Task<FileItemDto> Handle(SubmitChunksCommand request, CancellationToken cancellationToken)
        {
            var fileItem = await _fileItemService.GetAsync(request.UserId, request.FileItemId).ConfigureAwait(false);
            if (fileItem == null)
            {
                _logger.Error($"File item '{request.FileItemId}' was not found. [{request.UserId}]");

                throw new OperationErrorException(ErrorCode.EC101);
            }

            cancellationToken.ThrowIfCancellationRequested();

            var chunkList = await _uploadedChunkService.GetAllAsync(request.FileItemId, request.ApplicationId, cancellationToken).ConfigureAwait(false);
            var chunks = chunkList.ToList();
            cancellationToken.ThrowIfCancellationRequested();

            if (chunks.Count != request.ChunksCount)
            {
                _logger.Error($"Chunks count does not match. [{request.UserId}]");

                throw new OperationErrorException(ErrorCode.EC102);
            }

            var uploadedFileSource = new List<byte>();
            var chunksFileItemStoragePath = _fileAccessService.GetChunksFileItemStoragePath(request.FileItemId);
            foreach (var chunk in chunks.OrderBy(x => x.Order))
            {
                byte[] bytes;
                if (request.ChunksStorageSetting == StorageSetting.Database)
                {
                    bytes = chunk.Source;
                }
                else
                {
                    var filePath = Path.Combine(chunksFileItemStoragePath, chunk.Id.ToString());
                    bytes = await File.ReadAllBytesAsync(filePath, cancellationToken).ConfigureAwait(false);
                }

                uploadedFileSource.AddRange(bytes);
            }

            cancellationToken.ThrowIfCancellationRequested();
            var uploadedFile = await _fileItemService.UploadFileToStorageAsync(request.UserId, request.FileItemId, uploadedFileSource.ToArray()).ConfigureAwait(false);

            var totalTime = _fileItemService.GetAudioTotalTime(uploadedFile.FilePath);
            if (!totalTime.HasValue)
            {
                _fileItemService.CleanUploadedData(uploadedFile.DirectoryPath);

                _logger.Error($"File '{uploadedFile.FileName}' is not supported. [{request.UserId}]");

                throw new OperationErrorException(ErrorCode.EC201);
            }

            await _fileItemService.UpdateUploadStatusAsync(request.FileItemId, UploadStatus.InProgress, request.ApplicationId).ConfigureAwait(false);

            var dateUpdated = DateTime.UtcNow;
            var storageSetting = await _internalValueService.GetValueAsync(InternalValues.StorageSetting).ConfigureAwait(false);

            fileItem.ApplicationId = request.ApplicationId;
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

                await _fileItemService.UpdateUploadStatusAsync(fileItem.Id, UploadStatus.Completed, request.ApplicationId).ConfigureAwait(false);

                if (storageSetting == StorageSetting.Database)
                {
                    _fileItemService.CleanUploadedData(uploadedFile.DirectoryPath);
                }

                _logger.Information($"File item '{fileItem.Id}' was successfully submitted. [{request.UserId}]");
            }
            catch (DbUpdateException ex)
            {
                _fileItemService.CleanUploadedData(uploadedFile.DirectoryPath);

                _logger.Error($"Exception occurred during submitting file item chunks. Message: {ex.Message}. [{request.UserId}]");
                _logger.Error(ExceptionFormatter.FormatException(ex));

                throw new OperationErrorException(ErrorCode.EC400);
            }
            finally
            {
                await _uploadedChunkService.DeleteAsync(request.FileItemId, request.ApplicationId).ConfigureAwait(false);
            }

            return fileItem.ToDto();
        }
    }
}
