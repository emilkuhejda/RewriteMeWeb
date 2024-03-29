﻿using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Newtonsoft.Json;
using RewriteMe.Business.Configuration;
using RewriteMe.Business.Utils;
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
    public class CreateFileItemHandler : IRequestHandler<CreateFileItemCommand, FileItemDto>
    {
        private readonly IFileItemService _fileItemService;
        private readonly IInternalValueService _internalValueService;
        private readonly IMessageCenterService _messageCenterService;
        private readonly ILogger _logger;

        public CreateFileItemHandler(
            IFileItemService fileItemService,
            IInternalValueService internalValueService,
            IMessageCenterService messageCenterService,
            ILogger logger)
        {
            _fileItemService = fileItemService;
            _internalValueService = internalValueService;
            _messageCenterService = messageCenterService;
            _logger = logger.ForContext<CreateFileItemHandler>();
        }

        public async Task<FileItemDto> Handle(CreateFileItemCommand request, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrWhiteSpace(request.Language) && !SupportedLanguages.IsSupported(request.Language))
            {
                _logger.Error($"Language '{request.Language}' is not supported.");

                throw new OperationErrorException(ErrorCode.EC200);
            }

            var fileItemId = Guid.NewGuid();
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
                Storage = storageSetting,
                DateCreated = request.DateCreated,
                DateUpdatedUtc = dateUpdated
            };

            await _fileItemService.AddAsync(fileItem).ConfigureAwait(false);
            await _messageCenterService.SendAsync(HubMethodsHelper.GetFilesListChangedMethod(request.UserId)).ConfigureAwait(false);

            _logger.Information($"File item '{fileItem.Id}' was created. File item: {JsonConvert.SerializeObject(fileItem)}");

            return fileItem.ToDto();
        }
    }
}
