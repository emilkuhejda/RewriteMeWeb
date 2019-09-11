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
    public class UserRepository : IUserRepository
    {
        private readonly IDbContextFactory _contextFactory;

        public UserRepository(IDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<bool> ExistsAsync(Guid userId)
        {
            using (var context = _contextFactory.Create())
            {
                return await context.Users
                    .AsNoTracking()
                    .AnyAsync(x => x.Id == userId)
                    .ConfigureAwait(false);
            }
        }

        public async Task<bool> ExistsAsync(Guid userId, string email)
        {
            using (var context = _contextFactory.Create())
            {
                return await context.Users
                    .AsNoTracking()
                    .AnyAsync(x => x.Id == userId && x.Email == email)
                    .ConfigureAwait(false);
            }
        }

        public async Task AddAsync(User user)
        {
            using (var context = _contextFactory.Create())
            {
                var userEntity = user.ToUserEntity();
                await context.Users.AddAsync(userEntity).ConfigureAwait(false);
                await context.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        public async Task UpdateAsync(User user)
        {
            using (var context = _contextFactory.Create())
            {
                var userEntity = user.ToUserEntity();
                context.Entry(userEntity).State = EntityState.Modified;
                await context.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        public async Task<User> GetAsync(Guid userId)
        {
            using (var context = _contextFactory.Create())
            {
                var entity = await context.Users.SingleOrDefaultAsync(x => x.Id == userId).ConfigureAwait(false);
                return entity?.ToUser();
            }
        }

        public async Task<User> GetWithFilesAsync(Guid userId)
        {
            using (var context = _contextFactory.Create())
            {
                var entity = await context.Users
                    .AsNoTracking()
                    .Include(x => x.FileItems)
                    .SingleOrDefaultAsync(x => x.Id == userId)
                    .ConfigureAwait(false);

                return entity?.ToUser();
            }
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            using (var context = _contextFactory.Create())
            {
                var entities = await context.Users.ToListAsync().ConfigureAwait(false);
                return entities.Select(x => x.ToUser());
            }
        }

        public async Task DeleteAsync(User user)
        {
            using (var context = _contextFactory.Create())
            {
                var userEntity = user.ToUserEntity();
                context.Entry(userEntity).State = EntityState.Deleted;
                await context.SaveChangesAsync().ConfigureAwait(false);
            }
        }
    }
}
