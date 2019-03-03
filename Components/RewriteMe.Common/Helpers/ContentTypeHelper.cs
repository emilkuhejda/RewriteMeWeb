using System.Linq;

namespace RewriteMe.Common.Helpers
{
    public static class ContentTypeHelper
    {
        public static bool IsSupportedType(string contentType)
        {
            var supportedTypes = new[] { "audio/wav", "audio/x-m4a" };

            if (supportedTypes.Contains(contentType))
                return true;

            return false;
        }
    }
}
