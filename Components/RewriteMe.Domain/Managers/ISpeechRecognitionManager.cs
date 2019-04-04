using System;
using System.Threading.Tasks;
using RewriteMe.Domain.Transcription;

namespace RewriteMe.Domain.Managers
{
    public interface ISpeechRecognitionManager
    {
        Task<bool> CanRunRecognition(FileItem fileItem, Guid userId);

        void RunRecognition(FileItem fileItem, Guid userId);
    }
}
