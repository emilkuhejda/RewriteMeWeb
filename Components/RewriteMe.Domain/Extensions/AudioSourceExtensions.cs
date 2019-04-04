using RewriteMe.Common.Helpers;
using RewriteMe.Domain.Transcription;

namespace RewriteMe.Domain.Extensions
{
    public static class AudioSourceExtensions
    {
        public static bool IsWav(this AudioSource audioSource)
        {
            return audioSource.ContentType == "audio/wav";
        }

        public static bool IsSupportedType(this AudioSource audioSource)
        {
            return ContentTypeHelper.IsSupportedType(audioSource.ContentType);
        }
    }
}
