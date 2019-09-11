using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RewriteMe.Domain.UserManagement;

namespace RewriteMe.Domain.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<bool> ExistsAsync(Guid userId);

        Task<bool> ExistsAsync(Guid userId, string email);

        Task AddAsync(User user);

        Task UpdateAsync(User user);

        Task<User> GetAsync(Guid userId);

        Task<User> GetWithFilesAsync(Guid userId);

        Task<IEnumerable<User>> GetAllAsync();

        Task DeleteAsync(User user);
    }
}
