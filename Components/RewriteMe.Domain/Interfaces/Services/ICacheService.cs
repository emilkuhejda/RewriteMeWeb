using System;
using System.Threading.Tasks;
using RewriteMe.Domain.Enums;
using RewriteMe.Domain.Polling;

namespace RewriteMe.Domain.Interfaces.Services
{
    public interface ICacheService
    {
        CacheItem GetItem(Guid fileItemId);

        Task AddItemAsync(CacheItem cacheItem);

        Task UpdateRecognitionStateAsync(Guid fileItemId, RecognitionState recognitionState);

        Task UpdatePercentageAsync(Guid fileItemId, double percentage);

        Task RemoveItemAsync(Guid fileItemId);
    }
}
