using RewriteMe.Domain.Transcription;

namespace RewriteMe.Domain.Extensions
{
    public static class FileItemExtensions
    {
        public static bool IsOriginalWav(this FileItem fileItem)
        {
            return fileItem.OriginalContentType == "audio/wav";
        }
    }
}
