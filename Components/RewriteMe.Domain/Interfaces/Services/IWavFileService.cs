using System.Collections.Generic;
using System.Threading.Tasks;
using RewriteMe.Domain.Transcription;

namespace RewriteMe.Domain.Interfaces.Services
{
    public interface IWavFileService
    {
        WavFile CreateWavFileFromSource(byte[] source);

        Task<WavFile> ConvertToWavAsync(byte[] source);

        Task<IEnumerable<WavPartialFile>> SplitWavFileAsync(byte[] inputFile);
    }
}
