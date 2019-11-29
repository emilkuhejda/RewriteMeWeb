using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RewriteMe.Domain.Transcription;

namespace RewriteMe.Domain.Interfaces.Services
{
    public interface IWavFileService
    {
        (string outputFilePath, string fileName) CopyWavAsync(string directoryPath, string inputFilePath);

        Task<(string outputFilePath, string fileName)> ConvertToWavAsync(string directoryPath, string inputFilePath);

        Task<IEnumerable<WavPartialFile>> SplitWavFileAsync(byte[] inputFile, TimeSpan remainingTime);
    }
}
