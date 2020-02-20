using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using RewriteMe.Domain.Dtos;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.Domain.Recording;
using RewriteMe.WebApi.Commands;

namespace RewriteMe.WebApi.Handlers
{
    public class CreateSpeechConfigurationHandler : IRequestHandler<CreateSpeechConfigurationCommand, SpeechConfigurationDto>
    {
        private readonly IUserSubscriptionService _userSubscriptionService;
        private readonly IRecognizedAudioSampleService _recognizedAudioSampleService;
        private readonly IApplicationLogService _applicationLogService;

        public CreateSpeechConfigurationHandler(
            IUserSubscriptionService userSubscriptionService,
            IRecognizedAudioSampleService recognizedAudioSampleService,
            IApplicationLogService applicationLogService)
        {
            _userSubscriptionService = userSubscriptionService;
            _recognizedAudioSampleService = recognizedAudioSampleService;
            _applicationLogService = applicationLogService;
        }

        public async Task<SpeechConfigurationDto> Handle(CreateSpeechConfigurationCommand request, CancellationToken cancellationToken)
        {
            var recognizedAudioSample = new RecognizedAudioSample
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                DateCreatedUtc = DateTime.UtcNow
            };

            await _recognizedAudioSampleService.AddAsync(recognizedAudioSample).ConfigureAwait(false);

            var remainingTime = await _userSubscriptionService.GetRemainingTimeAsync(request.UserId).ConfigureAwait(false);
            var speechConfigurationDto = new SpeechConfigurationDto
            {
                SubscriptionKey = request.AppSettings.AzureSubscriptionKey,
                SpeechRegion = request.AppSettings.AzureSpeechRegion,
                AudioSampleId = recognizedAudioSample.Id,
                SubscriptionRemainingTimeTicks = remainingTime.Ticks
            };

            await _applicationLogService.InfoAsync($"User with ID='{request.UserId}' retrieved speech recognition configuration: {speechConfigurationDto}.", request.UserId).ConfigureAwait(false);

            return speechConfigurationDto;
        }
    }
}
