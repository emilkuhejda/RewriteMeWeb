using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Hangfire;
using MediatR;
using RewriteMe.Common.Utils;
using RewriteMe.Domain.Dtos;
using RewriteMe.Domain.Enums;
using RewriteMe.Domain.Exceptions;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.WebApi.Commands;
using Serilog;

namespace RewriteMe.WebApi.Handlers
{
    public class SendMailHandler : IRequestHandler<SendMailCommand, OkDto>
    {
        private const string Transcription = "Transcription";

        private readonly IFileItemService _fileItemService;
        private readonly ITranscribeItemService _transcribeItemService;
        private readonly IMailService _mailService;
        private readonly ILogger _logger;

        public SendMailHandler(
            IFileItemService fileItemService,
            ITranscribeItemService transcribeItemService,
            IMailService mailService,
            ILogger logger)
        {
            _fileItemService = fileItemService;
            _transcribeItemService = transcribeItemService;
            _mailService = mailService;
            _logger = logger;
        }

        public async Task<OkDto> Handle(SendMailCommand request, CancellationToken cancellationToken)
        {
            if (!RegexUtilities.IsValidEmail(request.Recipient))
            {
                _logger.Error($"Recipient '{request.FileItemId}' address format is incorrect.");

                throw new OperationErrorException(ErrorCode.EC202);
            }

            var fileItem = await _fileItemService.GetAsync(request.UserId, request.FileItemId).ConfigureAwait(false);
            if (fileItem == null)
            {
                _logger.Error($"File item '{request.FileItemId}' was not found.");

                throw new OperationErrorException(ErrorCode.EC101);
            }

            var transcribeItems = await _transcribeItemService.GetAllAsync(request.FileItemId).ConfigureAwait(false);

            var body = new StringBuilder();
            foreach (var transcribeItem in transcribeItems)
            {
                var transcript = string.IsNullOrWhiteSpace(transcribeItem.UserTranscript)
                    ? string.Join(string.Empty, transcribeItem.Alternatives.Select(x => x.Transcript))
                    : transcribeItem.UserTranscript;

                body.AppendLine(transcript);
                body.AppendLine();
            }

            _logger.Information($"Email for user '{request.UserId}' was sent to queue.");

            var subject = $"{Transcription}: {fileItem.Name}";
            BackgroundJob.Enqueue(() => _mailService.SendAsync(request.Recipient, subject, body.ToString()));

            return new OkDto();
        }
    }
}
