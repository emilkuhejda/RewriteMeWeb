using System;

namespace RewriteMe.Domain.Interfaces.Services
{
    public interface ICacheService
    {
        double GetPercentage(Guid fileItemId);

        void AddOrUpdateItem(Guid fileItemId, double percentage);

        void RemoveItem(Guid fileItemId);
    }
}
