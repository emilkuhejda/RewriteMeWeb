using System;
using System.Threading.Tasks;
using RewriteMe.Domain.Interfaces.Repositories;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.Domain.UserManagement;

namespace RewriteMe.Business.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> UserAlreadyExistsAsync(User user)
        {
            return await _userRepository.UserAlreadyExistsAsync(user).ConfigureAwait(false);
        }

        public async Task AddAsync(User user)
        {
            await _userRepository.AddAsync(user).ConfigureAwait(false);
        }

        public async Task<User> GetUserAsync(string username)
        {
            return await _userRepository.GetUserAsync(username).ConfigureAwait(false);
        }

        public async Task<User> GetUserAsync(Guid userId)
        {
            return await _userRepository.GetUserAsync(userId).ConfigureAwait(false);
        }
    }
}
