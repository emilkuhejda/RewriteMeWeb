using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RewriteMe.Business.Configuration;
using RewriteMe.Common.Utils;
using RewriteMe.Domain;
using RewriteMe.Domain.Dtos;
using RewriteMe.Domain.Enums;
using RewriteMe.Domain.Exceptions;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.Domain.Transcription;
using RewriteMe.WebApi.Commands;
using RewriteMe.WebApi.Extensions;
using Serilog;

namespace RewriteMe.WebApi.Handlers
{
    public class UploadFileSourceHandler : IRequestHandler<UploadFileSourceCommand, FileItemDto>
    {
        private readonly IFileItemService _fileItemService;
        private readonly IFileItemSourceService _fileItemSourceService;
        private readonly IInternalValueService _internalValueService;
        private readonly IMessageCenterService _messageCenterService;
        private readonly ILogger _logger;

        public UploadFileSourceHandler(
            IFileItemService fileItemService,
            IFileItemSourceService fileItemSourceService,
            IInternalValueService internalValueService,
            IMessageCenterService messageCenterService,
            ILogger logger)
        {
            _fileItemService = fileItemService;
            _fileItemSourceService = fileItemSourceService;
            _internalValueService = internalValueService;
            _messageCenterService = messageCenterService;
            _logger = logger.ForContext<UploadFileSourceHandler>();
        }

        public async Task<FileItemDto> Handle(UploadFileSourceCommand request, CancellationToken cancellationToken)
        {
            if (request.File == null)
            {
                _logger.Error($"Uploaded file source was not found. [{request.UserId}]");

                throw new OperationErrorException(ErrorCode.EC100);
            }

            if (!string.IsNullOrWhiteSpace(request.Language) && !SupportedLanguages.IsSupported(request.Language))
            {
                _logger.Error($"Language '{request.Language}' is not supported. [{request.UserId}]");

                throw new OperationErrorException(ErrorCode.EC200);
            }

            var fileItemId = Guid.NewGuid();
            var uploadedFileSource = await request.File.GetBytesAsync().ConfigureAwait(false);
            var uploadedFile = await _fileItemService.UploadFileToStorageAsync(request.UserId, fileItemId, uploadedFileSource).ConfigureAwait(false);

            var totalTime = _fileItemService.GetAudioTotalTime(uploadedFile.FilePath);
            if (!totalTime.HasValue)
            {
                _fileItemService.CleanUploadedData(uploadedFile.DirectoryPath);

                _logger.Error($"File '{request.FileName}' is not supported. [{request.UserId}]");

                throw new OperationErrorException(ErrorCode.EC201);
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
                await _fileItemService.UpdateUploadStatusAsync(fileItem.Id, UploadStatus.InProgress, request.ApplicationId).ConfigureAwait(false);

                if (storageSetting == StorageSetting.Database ||
                    await _internalValueService.GetValueAsync(InternalValues.IsDatabaseBackupEnabled).ConfigureAwait(false))
                {
                    await _fileItemSourceService.AddFileItemSourceAsync(fileItem, uploadedFile.FilePath).ConfigureAwait(false);
                }

                await _fileItemService.UpdateUploadStatusAsync(fileItem.Id, UploadStatus.Completed, request.ApplicationId).ConfigureAwait(false);

                _logger.Information($"File item '{fileItem.Id}' was created. File item: {JsonConvert.SerializeObject(fileItem)}");

                if (storageSetting == StorageSetting.Database)
                {
                    _fileItemService.CleanUploadedData(uploadedFile.DirectoryPath);
                }
            }
            catch (DbUpdateException ex)
            {
                _fileItemService.CleanUploadedData(uploadedFile.DirectoryPath);

                _logger.Error($"Exception occurred during uploading file item source. Message: {ex.Message}. [{request.UserId}]");
                _logger.Error(ExceptionFormatter.FormatException(ex));

                throw new OperationErrorException(ErrorCode.EC400);
            }

            return fileItem.ToDto();
        }
    }
}
