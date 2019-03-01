using System;
using System.Linq;
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

        public bool UserAlreadyExists(User user)
        {
            using (var context = _contextFactory.Create())
            {
                return context.Users.Any(x => x.Username == user.Username && x.Id != user.Id);
            }
        }

        public User GetUser(string username)
        {
            using (var context = _contextFactory.Create())
            {
                return context.Users.SingleOrDefault(x => x.Username == username)?.ToUser();
            }
        }

        public User GetUser(Guid userId)
        {
            using (var context = _contextFactory.Create())
            {
                return context.Users.SingleOrDefault(x => x.Id == userId)?.ToUser();
            }
        }

        public void Add(User user)
        {
            using (var context = _contextFactory.Create())
            {
                var userEntity = user.ToUserEntity();
                context.Users.Add(userEntity);
                context.SaveChanges();
            }
        }
    }
}
