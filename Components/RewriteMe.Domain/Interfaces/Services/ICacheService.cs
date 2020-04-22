using System;
using System.Threading.Tasks;
using RewriteMe.Domain.Polling;

namespace RewriteMe.Domain.Interfaces.Services
{
    public interface ICacheService
    {
        Task AddItem(Guid fileItemId, CacheItem cacheItem);

        Task UpdatePercentage(Guid fileItemId, double percentage);

        Task RemoveItem(Guid fileItemId);
    }
}
