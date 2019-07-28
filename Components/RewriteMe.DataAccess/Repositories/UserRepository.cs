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

        public async Task<bool> UserAlreadyExistsAsync(Guid userId)
        {
            using (var context = _contextFactory.Create())
            {
                return await context.Users.AnyAsync(x => x.Id == userId).ConfigureAwait(false);
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

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            using (var context = _contextFactory.Create())
            {
                var entites = await context.Users.ToListAsync().ConfigureAwait(false);
                return entites.Select(x => x.ToUser());
            }
        }
    }
}
