using System;
using System.Threading.Tasks;
using RewriteMe.Domain.Transcription;

namespace RewriteMe.Domain.Interfaces.Services
{
    public interface IAudioSourceService
    {
        byte[] GetAudioSource(Guid fileItemId);

        Task AddAsync(AudioSource audioSource);

        Task UpdateAsync(AudioSource audioSource);
    }
}
