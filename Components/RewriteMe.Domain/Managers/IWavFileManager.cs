using System;
using RewriteMe.Domain.Transcription;

namespace RewriteMe.Domain.Managers
{
    public interface IWavFileManager
    {
        void RunConversionToWav(FileItem fileItem, Guid userId);
    }
}
