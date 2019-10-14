using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RewriteMe.Domain.Interfaces.Repositories
{
    public interface ICleanUpRepository
    {
        Task<IEnumerable<Guid>> GetFileItemIdsForCleaningAsync(DateTime deleteBefore);

        Task CleanFileItemSourcesAsync(DateTime deleteBefore);

        Task CleanTranscribeItemSourceAsync(DateTime deleteBefore);
    }
}
