using System;
using System.Threading.Tasks;
using RewriteMe.Domain.Transcription;

namespace RewriteMe.Domain.Interfaces.Services
{
    public interface IAudioSourceService
    {
        byte[] GetAudioSource(Guid fileItemId);

        Task<TimeSpan> GetTotalTime(Guid fileItemId);

        Task<int> GetLastVersion(Guid userId);

        Task AddAsync(AudioSource audioSource);

        Task UpdateAsync(AudioSource audioSource);
    }
}
