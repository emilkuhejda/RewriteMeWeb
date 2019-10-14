using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RewriteMe.DataAccess.Seed;
using RewriteMe.Domain.Interfaces.Repositories;

namespace RewriteMe.DataAccess.Repositories
{
    public class DatabaseRepository : IDatabaseRepository
    {
        private readonly IDbContextFactory _contextFactory;

        public DatabaseRepository(IDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task ResetAsync()
        {
            using (var context = _contextFactory.Create())
            {
                await context.Database.EnsureDeletedAsync().ConfigureAwait(false);
                await context.Database.MigrateAsync().ConfigureAwait(false);

                DbSeed.Seed(context);
            }
        }
    }
}
