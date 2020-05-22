using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Newtonsoft.Json;
using RewriteMe.Business.Utils;
using RewriteMe.Domain;
using RewriteMe.Domain.Dtos;
using RewriteMe.Domain.Enums;
using RewriteMe.Domain.Exceptions;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.WebApi.Commands;
using RewriteMe.WebApi.Extensions;
using Serilog;

namespace RewriteMe.WebApi.Handlers
{
    public class UpdateFileItemHandler : IRequestHandler<UpdateFileItemCommand, FileItemDto>
    {
        private readonly IFileItemService _fileItemService;
        private readonly IMessageCenterService _messageCenterService;
        private readonly ILogger _logger;

        public UpdateFileItemHandler(
            IFileItemService fileItemService,
            IMessageCenterService messageCenterService,
            ILogger logger)
        {
            _fileItemService = fileItemService;
            _messageCenterService = messageCenterService;
            _logger = logger.ForContext<UpdateFileItemHandler>();
        }

        public async Task<FileItemDto> Handle(UpdateFileItemCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Language) || !SupportedLanguages.IsSupported(request.Language))
            {
                _logger.Error($"Language '{request.Language}' is not supported.");

                throw new OperationErrorException(ErrorCode.EC200);
            }

            var fileItem = await _fileItemService.GetAsync(request.UserId, request.FileItemId).ConfigureAwait(false);
            if (fileItem == null)
            {
                _logger.Error($"File item '{request.FileItemId}' was not found. [{request.UserId}]");

                throw new OperationErrorException(ErrorCode.EC101);
            }

            fileItem.ApplicationId = request.ApplicationId;
            fileItem.Name = request.Name;
            fileItem.Language = request.Language;
            fileItem.DateUpdatedUtc = DateTime.UtcNow;

            await _fileItemService.UpdateAsync(fileItem).ConfigureAwait(false);
            await _messageCenterService.SendAsync(HubMethodsHelper.GetFilesListChangedMethod(request.UserId)).ConfigureAwait(false);

            _logger.Information($"File item '{fileItem.Id}' was updated. File item: {JsonConvert.SerializeObject(fileItem)}");

            return fileItem.ToDto();
        }
    }
}
