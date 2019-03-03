using System.Threading.Tasks;
using RewriteMe.Domain.Settings;
using RewriteMe.Domain.Transcription;

namespace RewriteMe.Domain.Interfaces.Services
{
    public interface ISpeechRecognitionService
    {
        Task Recognize(FileItem fileItem, SpeechCredentials speechCredentials);
    }
}
