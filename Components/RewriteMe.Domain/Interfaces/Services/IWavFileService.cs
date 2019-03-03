using System.Collections.Generic;
using System.Threading.Tasks;

namespace RewriteMe.Domain.Interfaces.Services
{
    public interface IWavFileService
    {
        Task<byte[]> ConvertToWav(byte[] source);

        Task<IEnumerable<string>> SplitWavFile(byte[] inputFile);
    }
}
