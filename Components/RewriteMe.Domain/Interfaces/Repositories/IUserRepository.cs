using System;
using RewriteMe.Domain.UserManagement;

namespace RewriteMe.Domain.Interfaces.Repositories
{
    public interface IUserRepository
    {
        bool UserAlreadyExists(User user);

        User GetUser(string username);

        User GetUser(Guid userId);

        void Add(User user);
    }
}
