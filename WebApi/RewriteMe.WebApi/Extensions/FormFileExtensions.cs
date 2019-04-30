using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using NAudio.Wave;

namespace RewriteMe.WebApi.Extensions
{
    public static class FormFileExtensions
    {
        public static async Task<bool> IsSupportedType(this IFormFile formFile)
        {
            try
            {
                var source = await formFile.GetBytesAsync().ConfigureAwait(false);
                var inputFile = Path.GetTempFileName();
                await File.WriteAllBytesAsync(inputFile, source).ConfigureAwait(false);

                var reader = new MediaFoundationReader(inputFile);
                reader.Dispose();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
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
