using Microsoft.AspNetCore.Http;
using RewriteMe.Common.Helpers;

namespace RewriteMe.WebApi.Extensions
{
    public static class FormFileExtensions
    {
        public static bool IsSupportedType(this IFormFile formFile)
        {
            return ContentTypeHelper.IsSupportedType(formFile.ContentType);
        }
    }
}
