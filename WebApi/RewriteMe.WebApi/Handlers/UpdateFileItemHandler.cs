﻿using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Newtonsoft.Json;
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
    public class UpdateFileItemHandler : IRequestHandler<UpdateFileItemCommand, FileItemDto>
    {
        private readonly IFileItemService _fileItemService;
        private readonly ILogger _logger;

        public UpdateFileItemHandler(
            IFileItemService fileItemService,
            ILogger logger)
        {
            _fileItemService = fileItemService;
            _logger = logger;
        }

        public async Task<FileItemDto> Handle(UpdateFileItemCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Language) || !SupportedLanguages.IsSupported(request.Language))
            {
                _logger.Error($"[Update file item] Language '{request.Language}' is not supported.");

                throw new OperationErrorException(ErrorCode.EC200);
            }

            var fileItem = new FileItem
            {
                Id = request.FileItemId,
                UserId = request.UserId,
                ApplicationId = request.ApplicationId,
                Name = request.Name,
                Language = request.Language,
                DateUpdatedUtc = DateTime.UtcNow
            };

            await _fileItemService.UpdateAsync(fileItem).ConfigureAwait(false);

            _logger.Information($"[Update file item] File item '{fileItem.Id}' was updated. File item: {JsonConvert.SerializeObject(fileItem)}");

            return fileItem.ToDto();
        }
    }
}
