using System.Threading;
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
    public class TranscribeFileItemHandler : IRequestHandler<TranscribeFileItemCommand, OkDto>
    {
        private readonly IFileItemService _fileItemService;
        private readonly ISpeechRecognitionManager _speechRecognitionManager;
        private readonly ILogger _logger;

        public TranscribeFileItemHandler(
            IFileItemService fileItemService,
            ISpeechRecognitionManager speechRecognitionManager,
            ILogger logger)
        {
            _fileItemService = fileItemService;
            _speechRecognitionManager = speechRecognitionManager;
            _logger = logger;
        }

        public async Task<OkDto> Handle(TranscribeFileItemCommand request, CancellationToken cancellationToken)
        {
            var fileItemExists = await _fileItemService.ExistsAsync(request.UserId, request.FileItemId).ConfigureAwait(false);
            if (!fileItemExists)
            {
                _logger.Error($"[Transcribe file item] File item '{request.FileItemId}' not exists.");

                throw new OperationErrorException(ErrorCode.EC101);
            }

            if (!SupportedLanguages.IsSupported(request.Language))
            {
                _logger.Error($"[Transcribe file item] Language '{request.Language}' is not supported.");

                throw new OperationErrorException(ErrorCode.EC200);
            }

            var canRunRecognition = await _speechRecognitionManager.CanRunRecognition(request.UserId).ConfigureAwait(false);
            if (!canRunRecognition)
            {
                _logger.Error($"[Transcribe file item] User '{request.UserId}' has no enough left minutes in subscription.");

                throw new OperationErrorException(ErrorCode.EC300);
            }

            await _fileItemService.UpdateLanguageAsync(request.FileItemId, request.Language, request.ApplicationId).ConfigureAwait(false);

            _logger.Information($"[Transcribe file item] File item '{request.FileItemId}' updated language to '{request.Language}'.");

            return new OkDto();
        }
    }
}
