using System.Threading.Tasks;
using RewriteMe.Domain.Settings;

namespace RewriteMe.Domain.Interfaces.Services
{
    public interface ISpeechRecognitionService
    {
        Task Recognize(byte[] audioFile, SpeechCredentials speechCredentials);
    }
}
