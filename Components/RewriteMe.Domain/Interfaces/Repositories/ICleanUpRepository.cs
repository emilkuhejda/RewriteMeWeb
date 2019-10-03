using System;
using System.Threading.Tasks;

namespace RewriteMe.Domain.Interfaces.Repositories
{
    public interface ICleanUpRepository
    {
        Task CleanFileItemSourcesAsync(DateTime deleteBefore);

        Task CleanTranscribeItemSourceAsync(DateTime deleteBefore);
    }
}
