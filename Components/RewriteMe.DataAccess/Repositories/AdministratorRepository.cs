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

        public async Task<bool> AlreadyExists(Administrator administrator)
        {
            using (var context = _contextFactory.Create())
            {
                var entity = await context.Administrators
                    .SingleOrDefaultAsync(x => x.Username == administrator.Username && x.Id != administrator.Id)
                    .ConfigureAwait(false);

                return entity != null;
            }
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

        public async Task UpdateAsync(Administrator administrator)
        {
            using (var context = _contextFactory.Create())
            {
                var administratorEntity = administrator.ToAdministratorEntity();
                context.Attach(administratorEntity);
                context.Entry(administratorEntity).Property(x => x.FirstName).IsModified = true;
                context.Entry(administratorEntity).Property(x => x.LastName).IsModified = true;

                if (administratorEntity.PasswordHash != null)
                {
                    context.Entry(administratorEntity).Property(x => x.PasswordHash).IsModified = true;
                    context.Entry(administratorEntity).Property(x => x.PasswordSalt).IsModified = true;
                }

                await context.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        public async Task DeleteAsync(Guid administratorId)
        {
            using (var context = _contextFactory.Create())
            {
                var entity = await context.Administrators.FirstOrDefaultAsync(x => x.Id == administratorId).ConfigureAwait(false);
                if (entity == null)
                    return;

                context.Administrators.Remove(entity);
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
