using System;
using System.Collections.Generic;
using RewriteMe.Domain.Interfaces.Services;

namespace RewriteMe.Business.Services
{
    public class SpeechRecognitionCacheService : ISpeechRecognitionCacheService
    {
        private readonly Dictionary<Guid, double> _cache = new Dictionary<Guid, double>();
        private readonly object _lockObject = new object();

        public double GetPercentage(Guid fileItemId)
        {
            if (!_cache.ContainsKey(fileItemId))
                return 0;

            return _cache[fileItemId];
        }

        public void AddOrUpdateItem(Guid fileItemId, double percentage)
        {
            lock (_lockObject)
            {
                if (_cache.ContainsKey(fileItemId))
                {
                    _cache[fileItemId] = percentage;
                }
                else
                {
                    _cache.Add(fileItemId, percentage);
                }
            }
        }

        public void RemoveItem(Guid fileItemId)
        {
            if (_cache.ContainsKey(fileItemId))
            {
                _cache.Remove(fileItemId);
            }
        }
    }
}
