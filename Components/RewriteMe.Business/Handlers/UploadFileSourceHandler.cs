using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using RewriteMe.Business.Commands;
using RewriteMe.Business.Configuration;
using RewriteMe.Business.Extensions;
using RewriteMe.Common.Utils;
using RewriteMe.Domain;
using RewriteMe.Domain.Dtos;
using RewriteMe.Domain.Enums;
using RewriteMe.Domain.Exceptions;
using RewriteMe.Domain.Extensions;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.Domain.Transcription;

namespace RewriteMe.Business.Handlers
{
    public class UploadFileSourceHandler : IRequestHandler<UploadFileSourceCommand, FileItemDto>
    {
        private readonly IFileItemService _fileItemService;
        private readonly IFileItemSourceService _fileItemSourceService;
        private readonly IInternalValueService _internalValueService;
        private readonly IApplicationLogService _applicationLogService;

        public UploadFileSourceHandler(
            IFileItemService fileItemService,
            IFileItemSourceService fileItemSourceService,
            IInternalValueService internalValueService,
            IApplicationLogService applicationLogService)
        {
            _fileItemService = fileItemService;
            _fileItemSourceService = fileItemSourceService;
            _internalValueService = internalValueService;
            _applicationLogService = applicationLogService;
        }

        public async Task<FileItemDto> Handle(UploadFileSourceCommand request, CancellationToken cancellationToken)
        {
            if (request.File == null)
                throw new OperationErrorException(StatusCodes.Status400BadRequest, ErrorCode.EC100);

            if (!string.IsNullOrWhiteSpace(request.Language) && !SupportedLanguages.IsSupported(request.Language))
                throw new OperationErrorException(StatusCodes.Status400BadRequest, ErrorCode.EC200);

            var fileItemId = Guid.NewGuid();
            var uploadedFileSource = await request.File.GetBytesAsync().ConfigureAwait(false);
            var uploadedFile = await _fileItemService.UploadFileToStorageAsync(request.UserId, fileItemId, uploadedFileSource).ConfigureAwait(false);

            var totalTime = _fileItemService.GetAudioTotalTime(uploadedFile.FilePath);
            if (!totalTime.HasValue)
            {
                _fileItemService.CleanUploadedData(uploadedFile.DirectoryPath);

                throw new OperationErrorException(StatusCodes.Status400BadRequest, ErrorCode.EC201);
            }

            var dateUpdated = DateTime.UtcNow;
            var storageSetting = await _internalValueService.GetValueAsync(InternalValues.StorageSetting).ConfigureAwait(false);
            var fileItem = new FileItem
            {
                Id = fileItemId,
                UserId = request.UserId,
                ApplicationId = request.ApplicationId,
                Name = request.Name,
                FileName = request.FileName,
                Language = request.Language,
                OriginalSourceFileName = uploadedFile.FileName,
                Storage = storageSetting,
                TotalTime = totalTime.Value,
                DateCreated = request.DateCreated,
                DateUpdatedUtc = dateUpdated
            };

            try
            {
                await _fileItemService.AddAsync(fileItem).ConfigureAwait(false);
                await _fileItemService.UpdateUploadStatus(fileItem.Id, UploadStatus.InProgress, request.ApplicationId).ConfigureAwait(false);

                if (storageSetting == StorageSetting.Database ||
                    await _internalValueService.GetValueAsync(InternalValues.IsDatabaseBackupEnabled).ConfigureAwait(false))
                {
                    await _fileItemSourceService.AddFileItemSourceAsync(fileItem, uploadedFile.FilePath).ConfigureAwait(false);
                }

                await _fileItemService.UpdateUploadStatus(fileItem.Id, UploadStatus.Completed, request.ApplicationId).ConfigureAwait(false);

                if (storageSetting == StorageSetting.Database)
                {
                    _fileItemService.CleanUploadedData(uploadedFile.DirectoryPath);
                }
            }
            catch (DbUpdateException ex)
            {
                _fileItemService.CleanUploadedData(uploadedFile.DirectoryPath);

                await _applicationLogService.ErrorAsync(ExceptionFormatter.FormatException(ex), request.UserId).ConfigureAwait(false);

                throw new OperationErrorException(StatusCodes.Status400BadRequest, ErrorCode.EC400);
            }

            return fileItem.ToDto();
        }
    }
}
