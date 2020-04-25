using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Newtonsoft.Json;
using RewriteMe.Business.Utils;
using RewriteMe.Domain.Dtos;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.WebApi.Commands;
using Serilog;

namespace RewriteMe.WebApi.Handlers
{
    public class PermanentDeleteAllHandler : IRequestHandler<PermanentDeleteAllCommand, OkDto>
    {
        private readonly IFileItemService _fileItemService;
        private readonly IMessageCenterService _messageCenterService;
        private readonly ILogger _logger;

        public PermanentDeleteAllHandler(
            IFileItemService fileItemService,
            IMessageCenterService messageCenterService,
            ILogger logger)
        {
            _fileItemService = fileItemService;
            _messageCenterService = messageCenterService;
            _logger = logger.ForContext<PermanentDeleteAllHandler>();
        }

        public async Task<OkDto> Handle(PermanentDeleteAllCommand request, CancellationToken cancellationToken)
        {
            await _fileItemService.PermanentDeleteAllAsync(request.UserId, request.FileItemIds, request.ApplicationId).ConfigureAwait(false);
            await _messageCenterService.SendAsync(HubMethodsHelper.GetFilesListChangedMethod(request.UserId)).ConfigureAwait(false);

            _logger.Information($"File items '{JsonConvert.SerializeObject(request.FileItemIds)}' were permanently deleted.");

            return new OkDto();
        }
    }
}
