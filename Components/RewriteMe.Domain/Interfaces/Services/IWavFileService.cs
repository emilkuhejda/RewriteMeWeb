using System.Collections.Generic;
using System.Threading.Tasks;
using RewriteMe.Domain.Transcription;

namespace RewriteMe.Domain.Interfaces.Services
{
    public interface IWavFileService
    {
        Task<byte[]> ConvertToWav(byte[] source);

        Task<IEnumerable<WavFileItem>> SplitWavFile(byte[] inputFile);
    }
}
