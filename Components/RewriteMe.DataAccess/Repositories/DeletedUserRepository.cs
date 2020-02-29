using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RewriteMe.DataAccess.DataAdapters;
using RewriteMe.DataAccess.Entities;
using RewriteMe.Domain.Interfaces.Repositories;
using RewriteMe.Domain.UserManagement;

namespace RewriteMe.DataAccess.Repositories
{
    public class DeletedUserRepository : IDeletedUserRepository
    {
        private readonly IDbContextFactory _contextFactory;

        public DeletedUserRepository(IDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<IEnumerable<DeletedUser>> GetAllAsync()
        {
            using (var context = _contextFactory.Create())
            {
                var deletedUsers = await context.DeletedUsers.ToListAsync().ConfigureAwait(false);

                return deletedUsers.Select(x => x.ToDeletedUser());
            }
        }

        public async Task AddAsync(DeletedUser deletedUser)
        {
            using (var context = _contextFactory.Create())
            {
                await context.DeletedUsers.AddAsync(deletedUser.ToDeletedUserEntity()).ConfigureAwait(false);
                await context.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        public async Task DeleteAsync(Guid deletedUserId)
        {
            using (var context = _contextFactory.Create())
            {
                var deletedUserEntity = new DeletedUserEntity { Id = deletedUserId };
                context.Entry(deletedUserEntity).State = EntityState.Deleted;
                await context.SaveChangesAsync().ConfigureAwait(false);
            }
        }
    }
}
