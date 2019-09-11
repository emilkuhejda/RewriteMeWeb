using System.Collections.Generic;
using System.Threading.Tasks;
using RewriteMe.Domain.Transcription;

namespace RewriteMe.Domain.Interfaces.Services
{
    public interface ISpeechRecognitionService
    {
        Task<bool> CanCreateSpeechClientAsync();

        Task<IEnumerable<TranscribeItem>> Recognize(FileItem fileItem, IEnumerable<WavPartialFile> files);
    }
}
