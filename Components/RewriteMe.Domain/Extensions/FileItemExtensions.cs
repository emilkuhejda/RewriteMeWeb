using RewriteMe.Common.Helpers;
using RewriteMe.Domain.Transcription;

namespace RewriteMe.Domain.Extensions
{
    public static class FileItemExtensions
    {
        public static bool IsWav(this FileItem fileItem)
        {
            return fileItem.ContentType == "audio/wav";
        }

        public static bool IsSupportedType(this FileItem fileItem)
        {
            return ContentTypeHelper.IsSupportedType(fileItem.ContentType);
        }
    }
}
