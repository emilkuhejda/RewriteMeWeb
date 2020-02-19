using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using RewriteMe.Business.Commands;
using RewriteMe.Domain;
using RewriteMe.Domain.Dtos;
using RewriteMe.Domain.Enums;
using RewriteMe.Domain.Exceptions;
using RewriteMe.Domain.Interfaces.Managers;
using RewriteMe.Domain.Interfaces.Services;

namespace RewriteMe.Business.Handlers
{
    public class TranscribeFileItemHandler : IRequestHandler<TranscribeFileItemCommand, OkDto>
    {
        private readonly IFileItemService _fileItemService;
        private readonly ISpeechRecognitionManager _speechRecognitionManager;

        public TranscribeFileItemHandler(IFileItemService fileItemService, ISpeechRecognitionManager speechRecognitionManager)
        {
            _fileItemService = fileItemService;
            _speechRecognitionManager = speechRecognitionManager;
        }

        public async Task<OkDto> Handle(TranscribeFileItemCommand request, CancellationToken cancellationToken)
        {
            var fileItemExists = await _fileItemService.ExistsAsync(request.UserId, request.FileItemId).ConfigureAwait(false);
            if (!fileItemExists)
                throw new OperationErrorException(StatusCodes.Status400BadRequest, ErrorCode.EC101);

            if (!SupportedLanguages.IsSupported(request.Language))
                throw new OperationErrorException(StatusCodes.Status400BadRequest, ErrorCode.EC200);

            var canRunRecognition = await _speechRecognitionManager.CanRunRecognition(request.UserId).ConfigureAwait(false);
            if (!canRunRecognition)
                throw new OperationErrorException(StatusCodes.Status400BadRequest, ErrorCode.EC300);

            await _fileItemService.UpdateLanguageAsync(request.FileItemId, request.Language, request.ApplicationId).ConfigureAwait(false);

            return new OkDto();
        }
    }
}
