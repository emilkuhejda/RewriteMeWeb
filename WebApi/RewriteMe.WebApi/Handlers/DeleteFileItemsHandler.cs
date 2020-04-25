using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Newtonsoft.Json;
using RewriteMe.Business.Utils;
using RewriteMe.Domain.Dtos;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.WebApi.Commands;
using RewriteMe.WebApi.Extensions;
using Serilog;

namespace RewriteMe.WebApi.Handlers
{
    public class DeleteFileItemsHandler : IRequestHandler<DeleteFileItemsCommand, OkDto>
    {
        private readonly IFileItemService _fileItemService;
        private readonly IMessageCenterService _messageCenterService;
        private readonly ILogger _logger;

        public DeleteFileItemsHandler(
            IFileItemService fileItemService,
            IMessageCenterService messageCenterService,
            ILogger logger)
        {
            _fileItemService = fileItemService;
            _messageCenterService = messageCenterService;
            _logger = logger.ForContext<DeleteFileItemsHandler>();
        }

        public async Task<OkDto> Handle(DeleteFileItemsCommand request, CancellationToken cancellationToken)
        {
            var deletedFileItems = request.FileItems.Select(x => x.ToDeletedFileItem()).ToList();
            await _fileItemService.DeleteAllAsync(request.UserId, deletedFileItems, request.ApplicationId).ConfigureAwait(false);
            await _messageCenterService.SendAsync(HubMethodsHelper.GetFilesListChangedMethod(request.UserId)).ConfigureAwait(false);

            var fileItemIds = deletedFileItems.Select(x => x.Id).ToList();
            _logger.Information($"File items '{JsonConvert.SerializeObject(fileItemIds)}' were deleted.");

            return new OkDto();
        }
    }
}
