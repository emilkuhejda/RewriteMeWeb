using System.Threading.Tasks;
using RewriteMe.Domain.Transcription;

namespace RewriteMe.Domain.Managers
{
    public interface ISpeechRecognitionManager
    {
        Task RunRecognitionAsync(FileItem fileItem);
    }
}
