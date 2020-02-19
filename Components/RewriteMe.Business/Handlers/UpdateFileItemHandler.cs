using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using RewriteMe.Business.Commands;
using RewriteMe.Domain;
using RewriteMe.Domain.Dtos;
using RewriteMe.Domain.Enums;
using RewriteMe.Domain.Exceptions;
using RewriteMe.Domain.Extensions;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.Domain.Transcription;

namespace RewriteMe.Business.Handlers
{
    public class UpdateFileItemHandler : IRequestHandler<UpdateFileItemCommand, FileItemDto>
    {
        private readonly IFileItemService _fileItemService;

        public UpdateFileItemHandler(IFileItemService fileItemService)
        {
            _fileItemService = fileItemService;
        }

        public async Task<FileItemDto> Handle(UpdateFileItemCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Language) || !SupportedLanguages.IsSupported(request.Language))
                throw new OperationErrorException(ErrorCode.EC200);

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

            return fileItem.ToDto();
        }
    }
}
