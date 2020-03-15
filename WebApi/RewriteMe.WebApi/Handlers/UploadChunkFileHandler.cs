using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using RewriteMe.Domain.Dtos;
using RewriteMe.Domain.Enums;
using RewriteMe.Domain.Exceptions;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.WebApi.Commands;
using RewriteMe.WebApi.Extensions;
using Serilog;

namespace RewriteMe.WebApi.Handlers
{
    public class UploadChunkFileHandler : IRequestHandler<UploadChunkFileCommand, OkDto>
    {
        private readonly IUploadedChunkService _uploadedChunkService;
        private readonly ILogger _logger;

        public UploadChunkFileHandler(
            IUploadedChunkService uploadedChunkService,
            ILogger logger)
        {
            _uploadedChunkService = uploadedChunkService;
            _logger = logger;
        }

        public async Task<OkDto> Handle(UploadChunkFileCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.File == null)
                {
                    _logger.Error("[Upload chunk file] Uploaded file source was not found.");

                    throw new OperationErrorException(ErrorCode.EC100);
                }

                var uploadedFileSource = await request.File.GetBytesAsync(cancellationToken).ConfigureAwait(false);
                cancellationToken.ThrowIfCancellationRequested();

                await _uploadedChunkService.SaveAsync(request.FileItemId, request.Order, request.StorageSetting, request.ApplicationId, uploadedFileSource, cancellationToken).ConfigureAwait(false);

                _logger.Information($"[Upload chunk file] File chunk for file item '{request.FileItemId}' was uploaded.");

                return new OkDto();
            }
            catch (OperationCanceledException)
            {
                throw new OperationErrorException(ErrorCode.EC800);
            }
        }
    }
}
