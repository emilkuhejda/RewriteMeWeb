using System.Threading;
using System.Threading.Tasks;
using MediatR;
using RewriteMe.Business.Utils;
using RewriteMe.Domain.Dtos;
using RewriteMe.Domain.Enums;
using RewriteMe.Domain.Exceptions;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.WebApi.Commands;
using Serilog;

namespace RewriteMe.WebApi.Handlers
{
    public class DeleteFileItemHandler : IRequestHandler<DeleteFileItemCommand, OkDto>
    {
        private readonly IFileItemService _fileItemService;
        private readonly IMessageCenterService _messageCenterService;
        private readonly ILogger _logger;

        public DeleteFileItemHandler(
            IFileItemService fileItemService,
            IMessageCenterService messageCenterService,
            ILogger logger)
        {
            _fileItemService = fileItemService;
            _messageCenterService = messageCenterService;
            _logger = logger.ForContext<DeleteFileItemHandler>();
        }

        public async Task<OkDto> Handle(DeleteFileItemCommand request, CancellationToken cancellationToken)
        {
            var fileItem = await _fileItemService.GetAsync(request.UserId, request.FileItemId).ConfigureAwait(false);
            if (fileItem != null && fileItem.UploadStatus != UploadStatus.Completed)
            {
                _logger.Error($"File item source '{request.FileItemId}' is not uploaded. Uploaded state is '{fileItem.UploadStatus}'.");

                throw new OperationErrorException(ErrorCode.EC104);
            }

            await _fileItemService.DeleteAsync(request.UserId, request.FileItemId, request.ApplicationId).ConfigureAwait(false);
            await _messageCenterService.SendAsync(HubMethodsHelper.GetFilesListChangedMethod(request.UserId)).ConfigureAwait(false);

            _logger.Information($"File item '{request.FileItemId}' was deleted.");

            return new OkDto();
        }
    }
}
