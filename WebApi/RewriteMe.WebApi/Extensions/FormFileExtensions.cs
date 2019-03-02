using System.Linq;
using Microsoft.AspNetCore.Http;

namespace RewriteMe.WebApi.Extensions
{
    public static class FormFileExtensions
    {
        public static bool IsSupportedType(this IFormFile formFile)
        {
            var supportedTypes = new[] { "audio/wav", "audio/x-m4a" };

            if (supportedTypes.Contains(formFile.ContentType))
                return true;

            return false;
        }
    }
}
