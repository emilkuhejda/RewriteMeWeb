using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RewriteMe.DataAccess.DataAdapters;
using RewriteMe.Domain.Interfaces.Repositories;
using RewriteMe.Domain.UserManagement;

namespace RewriteMe.DataAccess.Repositories
{
    public class DeletedAccountRepository : IDeletedAccountRepository
    {
        private readonly IDbContextFactory _contextFactory;

        public DeletedAccountRepository(IDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<IEnumerable<DeletedAccount>> GetAllAsync()
        {
            using (var context = _contextFactory.Create())
            {
                var deletedAccounts = await context.DeletedAccounts.ToListAsync().ConfigureAwait(false);

                return deletedAccounts.Select(x => x.ToDeletedAccount());
            }
        }

        public async Task AddAsync(DeletedAccount deletedAccount)
        {
            using (var context = _contextFactory.Create())
            {
                await context.DeletedAccounts.AddAsync(deletedAccount.ToDeletedAccountEntity()).ConfigureAwait(false);
                await context.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        public async Task DeleteAsync(Guid deletedAccountId)
        {
            using (var context = _contextFactory.Create())
            {
                var deletedAccountEntity = await context.DeletedAccounts.SingleOrDefaultAsync(x => x.Id == deletedAccountId).ConfigureAwait(false);
                if (deletedAccountEntity == null)
                    return;

                context.DeletedAccounts.Remove(deletedAccountEntity);
                await context.SaveChangesAsync().ConfigureAwait(false);
            }
        }
    }
}
