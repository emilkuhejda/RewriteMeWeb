using System;
using System.Threading.Tasks;
using RewriteMe.Domain.Recording;

namespace RewriteMe.Domain.Interfaces.Repositories
{
    public interface IRecognizedAudioSampleRepository
    {
        Task AddAsync(RecognizedAudioSample recognizedAudioSample);

        Task<TimeSpan> GetRecognizedTime(Guid userId);
    }
}
