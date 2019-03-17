using System.Threading.Tasks;
using RewriteMe.DataAccess.DataAdapters;
using RewriteMe.Domain.Interfaces.Repositories;
using RewriteMe.Domain.Logging;

namespace RewriteMe.DataAccess.Repositories
{
    public class ApplicationLogRepository : IApplicationLogRepository
    {
        private readonly IDbContextFactory _contextFactory;

        public ApplicationLogRepository(IDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task AddAsync(ApplicationLog applicationLog)
        {
            using (var context = _contextFactory.Create())
            {
                await context.ApplicationLogs.AddAsync(applicationLog.ToApplicationLogEntity()).ConfigureAwait(false);
                await context.SaveChangesAsync().ConfigureAwait(false);
            }
        }
    }
}
