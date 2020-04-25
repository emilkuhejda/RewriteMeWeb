using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RewriteMe.Business.Extensions;
using RewriteMe.Business.Utils;
using RewriteMe.Domain.Enums;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.Domain.Polling;

namespace RewriteMe.Business.Services
{
    public class CacheService : ICacheService
    {
        private readonly IMessageCenterService _messageCenterService;

        private readonly Dictionary<Guid, CacheItem> _cache = new Dictionary<Guid, CacheItem>();
        private readonly object _lockUpdateRecognitionStateObject = new object();
        private readonly object _lockUpdatePercentageObject = new object();

        public CacheService(IMessageCenterService messageCenterService)
        {
            _messageCenterService = messageCenterService;
        }

        public CacheItem GetItem(Guid fileItemId)
        {
            try
            {
                return GetCacheItem(fileItemId);
            }
            catch (Exception)
            {
                return CacheItem.Empty;
            }
        }

        public async Task AddItemAsync(CacheItem cacheItem)
        {
            _cache.Add(cacheItem.FileItemId, cacheItem);

            await SendRecognitionStateChangedAsync(cacheItem, cacheItem.RecognitionState).ConfigureAwait(false);
        }

        public async Task UpdateRecognitionStateAsync(Guid fileItemId, RecognitionState recognitionState)
        {
            CacheItem cacheItem;
            lock (_lockUpdateRecognitionStateObject)
            {
                if (!_cache.ContainsKey(fileItemId))
                    throw new InvalidOperationException(nameof(fileItemId));

                cacheItem = _cache[fileItemId];
                cacheItem.RecognitionState = recognitionState;
            }

            await SendRecognitionStateChangedAsync(cacheItem, recognitionState).ConfigureAwait(false);
        }

        public async Task UpdatePercentageAsync(Guid fileItemId, double percentage)
        {
            lock (_lockUpdatePercentageObject)
            {
                if (!_cache.ContainsKey(fileItemId))
                    throw new InvalidOperationException(nameof(fileItemId));

                _cache[fileItemId].PercentageDone = percentage;
            }

            await SendProgressChangedAsync(fileItemId).ConfigureAwait(false);
        }

        public void RemoveItem(Guid fileItemId)
        {
            if (_cache.ContainsKey(fileItemId))
            {
                _cache.Remove(fileItemId);
            }
        }

        private CacheItem GetCacheItem(Guid fileItemId)
        {
            if (!_cache.ContainsKey(fileItemId))
                throw new InvalidOperationException(nameof(fileItemId));

            return _cache[fileItemId];
        }

        private async Task SendProgressChangedAsync(Guid fileItemId)
        {
            var cacheItem = GetCacheItem(fileItemId);
            await _messageCenterService.SendAsync(HubMethodsHelper.GetRecognitionProgressMethod(cacheItem.UserId), cacheItem.ToDto()).ConfigureAwait(false);
        }

        private async Task SendRecognitionStateChangedAsync(CacheItem cacheItem, RecognitionState recognitionState)
        {
            await _messageCenterService.SendAsync(HubMethodsHelper.GetRecognitionStateMethod(cacheItem.UserId), cacheItem.FileItemId, recognitionState.ToString()).ConfigureAwait(false);
        }
    }
}
