using System.Collections.Generic;
using System.Threading.Tasks;
using RewriteMe.Domain.Transcription;

namespace RewriteMe.Domain.Interfaces.Services
{
    public interface ISpeechRecognitionService
    {
        bool CanCreateSpeechClientAsync();

        Task<IEnumerable<TranscribeItem>> RecognizeAsync(FileItem fileItem, IEnumerable<WavPartialFile> files);
    }
}
