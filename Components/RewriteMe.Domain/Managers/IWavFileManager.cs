using System;
using RewriteMe.Domain.Transcription;

namespace RewriteMe.Domain.Managers
{
    public interface IWavFileManager
    {
        void RunConversionToWav(AudioSource audioSource, Guid userId);
    }
}
