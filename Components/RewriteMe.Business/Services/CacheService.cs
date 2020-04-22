using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using RewriteMe.Business.Polling;
using RewriteMe.Domain.Enums;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.Domain.Polling;

namespace RewriteMe.Business.Services
{
    public class CacheService : ICacheService
    {
        private readonly IHubContext<CacheHub> _cacheHub;

        private readonly Dictionary<Guid, CacheItem> _cache = new Dictionary<Guid, CacheItem>();
        private readonly object _lockObject = new object();

        public CacheService(IHubContext<CacheHub> cacheHub)
        {
            _cacheHub = cacheHub;
        }

        public async Task AddItem(Guid fileItemId, CacheItem cacheItem)
        {
            _cache.Add(fileItemId, cacheItem);

            await SendAsync(fileItemId).ConfigureAwait(false);
        }

        public async Task UpdatePercentage(Guid fileItemId, double percentage)
        {
            lock (_lockObject)
            {
                if (!_cache.ContainsKey(fileItemId))
                    throw new InvalidOperationException(nameof(fileItemId));

                _cache[fileItemId].PercentageDone = percentage;
            }

            await SendAsync(fileItemId).ConfigureAwait(false);
        }

        public async Task RemoveItem(Guid fileItemId)
        {
            var cacheItem = GetCacheItem(fileItemId).Copy();

            _cache.Remove(fileItemId);

            cacheItem.RecognitionState = RecognitionState.Completed;
            await SendAsync(cacheItem).ConfigureAwait(false);
        }

        private CacheItem GetCacheItem(Guid fileItemId)
        {
            if (!_cache.ContainsKey(fileItemId))
                throw new InvalidOperationException(nameof(fileItemId));

            return _cache[fileItemId];
        }

        private async Task SendAsync(Guid fileItemId)
        {
            var cacheItem = GetCacheItem(fileItemId);
            await SendAsync(cacheItem).ConfigureAwait(false);
        }

        private async Task SendAsync(CacheItem cacheItem)
        {
            await _cacheHub.Clients.All.SendAsync(cacheItem.UserId.ToString(), cacheItem).ConfigureAwait(false);
        }

    }
}
