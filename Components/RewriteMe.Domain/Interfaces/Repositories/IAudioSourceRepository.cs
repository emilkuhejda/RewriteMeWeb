using System;
using System.Threading.Tasks;
using RewriteMe.Domain.Transcription;

namespace RewriteMe.Domain.Interfaces.Repositories
{
    public interface IAudioSourceRepository
    {
        byte[] GetAudioSource(Guid fileItemId);

        Task<TimeSpan> GetTotalTime(Guid fileItemId);

        Task AddAsync(AudioSource audioSource);

        Task UpdateAsync(AudioSource audioSource);
    }
}
