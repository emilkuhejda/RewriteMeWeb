using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RewriteMe.DataAccess.Entities;
using RewriteMe.Domain.Interfaces.Repositories;

namespace RewriteMe.DataAccess.Repositories
{
    public class InternalValueRepository : IInternalValueRepository
    {
        private readonly IDbContextFactory _contextFactory;

        public InternalValueRepository(IDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<string> GetValue(string key)
        {
            using (var context = _contextFactory.Create())
            {
                var entity = await context.InternalValues.FirstOrDefaultAsync(x => x.Key == key).ConfigureAwait(false);
                return entity?.Value;
            }
        }

        public async Task UpdateValue(string key, string value)
        {
            using (var context = _contextFactory.Create())
            {
                var entity = await context.InternalValues.FirstOrDefaultAsync(x => x.Key == key).ConfigureAwait(false);
                if (entity == null)
                {
                    entity = new InternalValueEntity { Key = key, Value = value };
                    await context.InternalValues.AddAsync(entity).ConfigureAwait(false);
                }
                else
                {
                    entity.Value = value;
                }

                await context.SaveChangesAsync().ConfigureAwait(false);
            }
        }
    }
}
