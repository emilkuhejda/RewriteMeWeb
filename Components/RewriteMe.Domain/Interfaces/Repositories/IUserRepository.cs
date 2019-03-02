using System;
using System.Threading.Tasks;
using RewriteMe.Domain.UserManagement;

namespace RewriteMe.Domain.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<bool> UserAlreadyExistsAsync(User user);

        Task<User> GetUserAsync(string username);

        Task<User> GetUserAsync(Guid userId);

        Task AddAsync(User user);
    }
}
