﻿using System.Threading;
using System.Threading.Tasks;
using MediatR;
using RewriteMe.Domain;
using RewriteMe.Domain.Dtos;
using RewriteMe.Domain.Enums;
using RewriteMe.Domain.Exceptions;
using RewriteMe.Domain.Interfaces.Managers;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.WebApi.Commands;
using Serilog;

namespace RewriteMe.WebApi.Handlers
{
    public class UpdateFileItemLanguageHandler : IRequestHandler<UpdateFileItemLanguageCommand, OkDto>
    {
        private readonly IFileItemService _fileItemService;
        private readonly ISpeechRecognitionManager _speechRecognitionManager;
        private readonly ILogger _logger;

        public UpdateFileItemLanguageHandler(
            IFileItemService fileItemService,
            ISpeechRecognitionManager speechRecognitionManager,
            ILogger logger)
        {
            _fileItemService = fileItemService;
            _speechRecognitionManager = speechRecognitionManager;
            _logger = logger.ForContext<UpdateFileItemLanguageHandler>();
        }

        public async Task<OkDto> Handle(UpdateFileItemLanguageCommand request, CancellationToken cancellationToken)
        {
            var fileItemExists = await _fileItemService.ExistsAsync(request.UserId, request.FileItemId).ConfigureAwait(false);
            if (!fileItemExists)
            {
                _logger.Error($"File item '{request.FileItemId}' not exists.");

                throw new OperationErrorException(ErrorCode.EC101);
            }

            if (!SupportedLanguages.IsSupported(request.Language))
            {
                _logger.Error($"Language '{request.Language}' is not supported.");

                throw new OperationErrorException(ErrorCode.EC200);
            }

            var canRunRecognition = await _speechRecognitionManager.CanRunRecognition(request.UserId).ConfigureAwait(false);
            if (!canRunRecognition)
            {
                _logger.Error($"User '{request.UserId}' has no enough left minutes in subscription.");

                throw new OperationErrorException(ErrorCode.EC300);
            }

            await _fileItemService.UpdateLanguageAsync(request.FileItemId, request.Language, request.ApplicationId).ConfigureAwait(false);

            _logger.Information($"File item '{request.FileItemId}' updated language to '{request.Language}'.");

            return new OkDto();
        }
    }
}