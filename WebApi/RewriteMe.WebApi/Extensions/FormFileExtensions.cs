using System.IO;
using System.Threading.Tasks;
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

        public static async Task<byte[]> GetBytesAsync(this IFormFile formFile)
        {
            using (var memoryStream = new MemoryStream())
            {
                await formFile.CopyToAsync(memoryStream).ConfigureAwait(false);

                return memoryStream.ToArray();
            }
        }
    }
}
