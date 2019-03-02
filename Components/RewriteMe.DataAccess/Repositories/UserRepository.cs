using System;
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

        public async Task<bool> UserAlreadyExistsAsync(User user)
        {
            using (var context = _contextFactory.Create())
            {
                return await context.Users.AnyAsync(x => x.Username == user.Username && x.Id != user.Id).ConfigureAwait(false);
            }
        }

        public async Task<User> GetUserAsync(string username)
        {
            using (var context = _contextFactory.Create())
            {
                var user = await context.Users.SingleOrDefaultAsync(x => x.Username == username).ConfigureAwait(false);
                return user?.ToUser();
            }
        }

        public async Task<User> GetUserAsync(Guid userId)
        {
            using (var context = _contextFactory.Create())
            {
                var user = await context.Users.SingleOrDefaultAsync(x => x.Id == userId).ConfigureAwait(false);
                return user?.ToUser();
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
    }
}
