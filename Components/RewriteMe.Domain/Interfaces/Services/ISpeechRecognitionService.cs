using System.Collections.Generic;
using System.Threading.Tasks;
using RewriteMe.Domain.Transcription;

namespace RewriteMe.Domain.Interfaces.Services
{
    public interface ISpeechRecognitionService
    {
        bool CanCreateSpeechClientAsync();

        Task RecognizeAsync(FileItem fileItem, IEnumerable<WavPartialFile> files);
    }
}
