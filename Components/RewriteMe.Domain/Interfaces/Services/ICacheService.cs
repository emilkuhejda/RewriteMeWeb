using System;
using System.Threading.Tasks;
using RewriteMe.Domain.Polling;

namespace RewriteMe.Domain.Interfaces.Services
{
    public interface ICacheService
    {
        CacheItem GetItem(Guid fileItemId);

        Task AddItemAsync(Guid fileItemId, CacheItem cacheItem);

        Task UpdatePercentageAsync(Guid fileItemId, double percentage);

        Task RemoveItemAsync(Guid fileItemId);
    }
}
