using System;
using System.Threading.Tasks;
using RewriteMe.Domain.Interfaces.Repositories;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.Domain.Transcription;

namespace RewriteMe.Business.Services
{
    public class AudioSourceService : IAudioSourceService
    {
        private readonly IAudioSourceRepository _audioSourceRepository;

        public AudioSourceService(IAudioSourceRepository audioSourceRepository)
        {
            _audioSourceRepository = audioSourceRepository;
        }

        public byte[] GetAudioSource(Guid fileItemId)
        {
            return _audioSourceRepository.GetAudioSource(fileItemId);
        }

        public async Task<TimeSpan> GetTotalTime(Guid fileItemId)
        {
            return await _audioSourceRepository.GetTotalTime(fileItemId).ConfigureAwait(false);
        }

        public async Task<int> GetLastVersion(Guid userId)
        {
            return await _audioSourceRepository.GetLastVersion(userId).ConfigureAwait(false);
        }

        public async Task AddAsync(AudioSource audioSource)
        {
            await _audioSourceRepository.AddAsync(audioSource).ConfigureAwait(false);
        }

        public async Task UpdateAsync(AudioSource audioSource)
        {
            await _audioSourceRepository.UpdateAsync(audioSource).ConfigureAwait(false);
        }
    }
}
