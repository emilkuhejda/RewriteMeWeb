using System.Threading.Tasks;
using RewriteMe.Domain.Interfaces.Repositories;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.Domain.Recording;
using Serilog;

namespace RewriteMe.Business.Services
{
    public class RecognizedAudioSampleService : IRecognizedAudioSampleService
    {
        private readonly IRecognizedAudioSampleRepository _recognizedAudioSampleRepository;
        private readonly ILogger _logger;

        public RecognizedAudioSampleService(
            IRecognizedAudioSampleRepository recognizedAudioSampleRepository,
            ILogger logger)
        {
            _recognizedAudioSampleRepository = recognizedAudioSampleRepository;
            _logger = logger.ForContext<RecognizedAudioSampleService>();
        }

        public async Task AddAsync(RecognizedAudioSample recognizedAudioSample)
        {
            await _recognizedAudioSampleRepository.AddAsync(recognizedAudioSample).ConfigureAwait(false);

            _logger.Information($"Create recognized audio sample '{recognizedAudioSample.Id}'.");
        }
    }
}
