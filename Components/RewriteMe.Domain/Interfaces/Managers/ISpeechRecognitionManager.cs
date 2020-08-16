using System;
using System.Threading.Tasks;

namespace RewriteMe.Domain.Interfaces.Managers
{
    public interface ISpeechRecognitionManager
    {
        Task<bool> CanRunRecognition(Guid userId);

        Task RunRecognitionAsync(Guid userId, Guid fileItemId);

        Task RunRecognitionAsync(Guid userId, Guid fileItemId, bool isRestarted);
    }
}
