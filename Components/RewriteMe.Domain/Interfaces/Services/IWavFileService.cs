using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RewriteMe.Domain.Transcription;

namespace RewriteMe.Domain.Interfaces.Services
{
    public interface IWavFileService
    {
        string CopyWavAsync(FileItem fileItem);

        Task<string> ConvertToWavAsync(FileItem fileItem);

        Task<IEnumerable<WavPartialFile>> SplitWavFileAsync(byte[] inputFile, TimeSpan remainingTime);
    }
}
