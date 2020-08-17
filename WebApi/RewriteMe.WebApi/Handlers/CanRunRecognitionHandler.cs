using System.Threading;
using System.Threading.Tasks;
using MediatR;
using RewriteMe.Business.Utils;
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
    public class CanRunRecognitionHandler : IRequestHandler<CanRunRecognitionCommand, OkDto>
    {
        private readonly IFileItemService _fileItemService;
        private readonly ISpeechRecognitionManager _speechRecognitionManager;
        private readonly ILogger _logger;

        public CanRunRecognitionHandler(
            IFileItemService fileItemService,
            ISpeechRecognitionManager speechRecognitionManager,
            ILogger logger)
        {
            _fileItemService = fileItemService;
            _speechRecognitionManager = speechRecognitionManager;
            _logger = logger.ForContext<CanRunRecognitionHandler>();
        }

        public async Task<OkDto> Handle(CanRunRecognitionCommand request, CancellationToken cancellationToken)
        {
            var fileItem = await _fileItemService.GetAsync(request.UserId, request.FileItemId).ConfigureAwait(false);
            if (fileItem == null)
            {
                _logger.Error($"File item '{request.FileItemId}' not exists.");

                throw new OperationErrorException(ErrorCode.EC101);
            }

            if (ProcessingJobs.AnyJob(request.UserId))
            {
                _logger.Error($"User try to run more then one file recognition.");

                throw new OperationErrorException(ErrorCode.EC303);
            }

            if (fileItem.UploadStatus != UploadStatus.Completed)
            {
                _logger.Error($"File item source '{request.FileItemId}' is not uploaded. Uploaded state is '{fileItem.UploadStatus}'.");

                throw new OperationErrorException(ErrorCode.EC104);
            }

            if (fileItem.RecognitionState != RecognitionState.None)
            {
                _logger.Error($"File item '{request.FileItemId}' is in wrong recognition state. Recognition state is '{fileItem.RecognitionState}'.");

                throw new OperationErrorException(ErrorCode.EC103);
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
