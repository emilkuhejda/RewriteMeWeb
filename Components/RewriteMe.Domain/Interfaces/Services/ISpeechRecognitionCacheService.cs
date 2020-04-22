using System;

namespace RewriteMe.Domain.Interfaces.Services
{
    public interface ISpeechRecognitionCacheService
    {
        double GetPercentage(Guid fileItemId);

        void AddItem(Guid fileItemId);

        void RemoveItem(Guid fileItemId);
    }
}
