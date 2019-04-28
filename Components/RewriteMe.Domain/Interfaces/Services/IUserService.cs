using System;
using System.Threading.Tasks;
using RewriteMe.Domain.UserManagement;

namespace RewriteMe.Domain.Interfaces.Services
{
    public interface IUserService
    {
        Task<bool> UserAlreadyExistsAsync(Guid userId);

        Task AddAsync(User user);
    }
}
