using System;
using RewriteMe.Domain.UserManagement;

namespace RewriteMe.Domain.Interfaces.Services
{
    public interface IUserService
    {
        bool UserAlreadyExists(User user);

        void Add(User user);

        User GetUser(string username);

        User GetUser(Guid userId);
    }
}
