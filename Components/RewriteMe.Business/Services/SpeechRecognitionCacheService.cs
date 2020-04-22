using System;
using RewriteMe.Domain.Interfaces.Services;

namespace RewriteMe.Business.Services
{
    public class SpeechRecognitionCacheService : ISpeechRecognitionCacheService
    {
        public double GetPercentage(Guid fileItemId)
        {
            return 10;
        }

        public void AddItem(Guid fileItemId)
        {
            throw new NotImplementedException();
        }

        public void RemoveItem(Guid fileItemId)
        {
            throw new NotImplementedException();
        }
    }
}
