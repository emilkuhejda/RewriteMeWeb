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

namespace RewriteMe.WebApi.Handlers
{
    public class UploadChunkFileHandler : IRequestHandler<UploadChunkFileCommand, OkDto>
    {
        private readonly IUploadedChunkService _uploadedChunkService;

        public UploadChunkFileHandler(IUploadedChunkService uploadedChunkService)
        {
            _uploadedChunkService = uploadedChunkService;
        }

        public async Task<OkDto> Handle(UploadChunkFileCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.File == null)
                    throw new OperationErrorException(ErrorCode.EC100);

                var uploadedFileSource = await request.File.GetBytesAsync(cancellationToken).ConfigureAwait(false);
                cancellationToken.ThrowIfCancellationRequested();

                await _uploadedChunkService.SaveAsync(request.FileItemId, request.Order, request.StorageSetting, request.ApplicationId, uploadedFileSource, cancellationToken).ConfigureAwait(false);

                return new OkDto();
            }
            catch (OperationCanceledException)
            {
                throw new OperationErrorException(ErrorCode.EC800);
            }
        }
    }
}
