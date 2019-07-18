using System.Threading.Tasks;
using RewriteMe.Domain.Recording;

namespace RewriteMe.Domain.Interfaces.Services
{
    public interface IRecognizedAudioSampleService
    {
        Task AddAsync(RecognizedAudioSample recognizedAudioSample);
    }
}
