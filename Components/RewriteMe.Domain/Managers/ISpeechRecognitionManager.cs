using System;
using RewriteMe.Domain.Transcription;

namespace RewriteMe.Domain.Managers
{
    public interface ISpeechRecognitionManager
    {
        void RunRecognition(FileItem fileItem, Guid userId);
    }
}
