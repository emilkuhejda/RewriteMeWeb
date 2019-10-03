using System;
using System.Threading.Tasks;
using RewriteMe.Domain.Interfaces.Services;

namespace RewriteMe.Business.Services
{
    public class CleanUpService : ICleanUpService
    {
        public async Task CleanAsync(DateTime deleteBefore)
        {
            await Task.CompletedTask.ConfigureAwait(false);
        }
    }
}
