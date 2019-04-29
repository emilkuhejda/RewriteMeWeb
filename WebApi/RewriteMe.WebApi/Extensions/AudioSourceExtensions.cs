using RewriteMe.Domain.Transcription;
using RewriteMe.WebApi.Dtos;

namespace RewriteMe.WebApi.Extensions
{
    public static class AudioSourceExtensions
    {
        public static AudioSourceDto ToDto(this AudioSource audioSource)
        {
            return new AudioSourceDto
            {
                Id = audioSource.Id,
                FileItemId = audioSource.FileItemId,
                ContentType = audioSource.ContentType,
                Version = audioSource.Version
            };
        }
    }
}
