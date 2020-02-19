using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using RewriteMe.Business.Commands;
using RewriteMe.Business.Configuration;
using RewriteMe.Domain;
using RewriteMe.Domain.Dtos;
using RewriteMe.Domain.Enums;
using RewriteMe.Domain.Exceptions;
using RewriteMe.Domain.Extensions;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.Domain.Transcription;

namespace RewriteMe.Business.Handlers
{
    public class CreateFileItemHandler : IRequestHandler<CreateFileItemCommand, FileItemDto>
    {
        private readonly IFileItemService _fileItemService;
        private readonly IInternalValueService _internalValueService;

        public CreateFileItemHandler(
            IFileItemService fileItemService,
            IInternalValueService internalValueService)
        {
            _fileItemService = fileItemService;
            _internalValueService = internalValueService;
        }

        public async Task<FileItemDto> Handle(CreateFileItemCommand request, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrWhiteSpace(request.Language) && !SupportedLanguages.IsSupported(request.Language))
                throw new OperationErrorException(ErrorCode.EC200);

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

            return fileItem.ToDto();
        }
    }
}
