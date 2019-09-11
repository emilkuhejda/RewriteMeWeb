using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RewriteMe.Domain.UserManagement;

namespace RewriteMe.Domain.Interfaces.Services
{
    public interface IUserService
    {
        Task<bool> ExistsAsync(Guid userId);

        Task<bool> ExistsAsync(Guid userId, string email);

        Task AddAsync(User user);

        Task UpdateAsync(User user);

        Task<User> GetAsync(Guid userId);

        Task<IEnumerable<User>> GetAllAsync();

        Task<bool> DeleteAsync(Guid userId);
    }
}
