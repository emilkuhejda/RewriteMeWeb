using System.Threading.Tasks;
using RewriteMe.Domain.Interfaces.Repositories;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.Domain.Recording;

namespace RewriteMe.Business.Services
{
    public class RecognizedAudioSampleService : IRecognizedAudioSampleService
    {
        private readonly IRecognizedAudioSampleRepository _recognizedAudioSampleRepository;

        public RecognizedAudioSampleService(IRecognizedAudioSampleRepository recognizedAudioSampleRepository)
        {
            _recognizedAudioSampleRepository = recognizedAudioSampleRepository;
        }

        public async Task AddAsync(RecognizedAudioSample recognizedAudioSample)
        {
            await _recognizedAudioSampleRepository.AddAsync(recognizedAudioSample).ConfigureAwait(false);
        }
    }
}
