using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RewriteMe.Domain.Transcription;

namespace RewriteMe.Domain.Interfaces.Managers
{
    public interface IWavFileManager
    {
        Task RunConversionToWavAsync(FileItem fileItem, Guid userId);

        Task<IEnumerable<WavPartialFile>> SplitFileItemSourceAsync(Guid fileItemId, TimeSpan remainingTime);
    }
}
