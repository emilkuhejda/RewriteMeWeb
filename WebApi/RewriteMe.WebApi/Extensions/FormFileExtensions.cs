using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using NAudio.Wave;

namespace RewriteMe.WebApi.Extensions
{
    public static class FormFileExtensions
    {
        public static async Task<TimeSpan> GetTotalTime(this IFormFile formFile)
        {
            var source = await formFile.GetBytesAsync().ConfigureAwait(false);
            var inputFile = Path.GetTempFileName();
            await File.WriteAllBytesAsync(inputFile, source).ConfigureAwait(false);

            using (var reader = new MediaFoundationReader(inputFile))
            {
                return reader.TotalTime;
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
