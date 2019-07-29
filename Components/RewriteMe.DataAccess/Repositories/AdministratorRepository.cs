using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RewriteMe.DataAccess.DataAdapters;
using RewriteMe.Domain.Administration;
using RewriteMe.Domain.Interfaces.Repositories;

namespace RewriteMe.DataAccess.Repositories
{
    public class AdministratorRepository : IAdministratorRepository
    {
        private readonly IDbContextFactory _contextFactory;

        public AdministratorRepository(IDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task AddAsync(Administrator administrator)
        {
            using (var context = _contextFactory.Create())
            {
                var administratorEntity = administrator.ToAdministratorEntity();
                await context.Administrators.AddAsync(administratorEntity).ConfigureAwait(false);
                await context.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        public async Task<IEnumerable<Administrator>> GetAllAsync()
        {
            using (var context = _contextFactory.Create())
            {
                var entities = await context.Administrators.ToListAsync().ConfigureAwait(false);
                return entities.Select(x => x.ToAdministrator());
            }
        }

        public async Task<Administrator> GetAsync(string username)
        {
            using (var context = _contextFactory.Create())
            {
                var administrator = await context.Administrators.SingleOrDefaultAsync(x => x.Username == username).ConfigureAwait(false);
                return administrator?.ToAdministrator();
            }
        }

        public async Task<Administrator> GetAsync(Guid administratorId)
        {
            using (var context = _contextFactory.Create())
            {
                var administrator = await context.Administrators.SingleOrDefaultAsync(x => x.Id == administratorId).ConfigureAwait(false);
                return administrator?.ToAdministrator();
            }
        }
    }
}
