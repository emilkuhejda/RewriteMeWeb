using RewriteMe.Domain.Transcription;

namespace RewriteMe.Domain.Extensions
{
    public static class AudioSourceExtensions
    {
        public static bool IsWav(this AudioSource audioSource)
        {
            return audioSource.ContentType == "audio/wav";
        }
    }
}
