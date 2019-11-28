using System;
using System.Threading.Tasks;

namespace RewriteMe.Domain.Interfaces.Managers
{
    public interface ISpeechRecognitionManager
    {
        Task<bool> CanRunRecognition(Guid userId);

        void RunRecognition(Guid userId, Guid fileItemId);
    }
}
