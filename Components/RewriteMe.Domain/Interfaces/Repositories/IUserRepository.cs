using System;
using System.Threading.Tasks;
using RewriteMe.Domain.UserManagement;

namespace RewriteMe.Domain.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<bool> UserAlreadyExistsAsync(Guid userId);

        Task AddAsync(User user);
    }
}
