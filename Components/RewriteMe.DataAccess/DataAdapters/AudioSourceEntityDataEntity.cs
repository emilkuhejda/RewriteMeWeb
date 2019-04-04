using RewriteMe.DataAccess.Entities;
using RewriteMe.Domain.Transcription;

namespace RewriteMe.DataAccess.DataAdapters
{
    public static class AudioSourceEntityDataEntity
    {
        public static AudioSource ToAudioSource(this AudioSourceEntity entity)
        {
            return new AudioSource
            {
                Id = entity.Id,
                FileItemId = entity.FileItemId,
                OriginalSource = entity.OriginalSource,
                WavSource = entity.WavSource,
                ContentType = entity.ContentType,
                TotalTime = entity.TotalTime
            };
        }

        public static AudioSourceEntity ToAudioSourceEntity(this AudioSource audioSource)
        {
            return new AudioSourceEntity
            {
                Id = audioSource.Id,
                FileItemId = audioSource.FileItemId,
                OriginalSource = audioSource.OriginalSource,
                WavSource = audioSource.WavSource,
                ContentType = audioSource.ContentType,
                TotalTime = audioSource.TotalTime
            };
        }
    }
}
