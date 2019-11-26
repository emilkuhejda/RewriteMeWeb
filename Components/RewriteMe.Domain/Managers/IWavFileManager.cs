using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RewriteMe.Domain.Transcription;

namespace RewriteMe.Domain.Managers
{
    public interface IWavFileManager
    {
        Task RunConversionToWavAsync(FileItem fileItem, Guid userId);

        Task<IEnumerable<WavPartialFile>> SplitFileItemSourceAsync(FileItem fileItem, TimeSpan remainingTime);
    }
}
