using System.IO;
using System.Threading.Tasks;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.Domain.Settings;

namespace RewriteMe.Business.Services
{
    public class SpeechRecognitionService : ISpeechRecognitionService
    {
        private readonly IWavFileService _wavFileService;

        public SpeechRecognitionService(IWavFileService wavFileService)
        {
            _wavFileService = wavFileService;
        }

        public async Task Recognize(byte[] audioFile, SpeechCredentials speechCredentials)
        {
            var files = await _wavFileService.SplitWavFile(audioFile).ConfigureAwait(false);

            foreach (var file in files)
            {
                if (File.Exists(file))
                    File.Delete(file);
            }

            await Task.CompletedTask.ConfigureAwait(false);
        }
    }
}
