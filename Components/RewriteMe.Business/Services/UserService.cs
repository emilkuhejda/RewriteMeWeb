using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using RewriteMe.Domain.Interfaces.Repositories;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.Domain.UserManagement;

namespace RewriteMe.Business.Services
{
    public class UserService : IUserService
    {
        private readonly IStorageService _storageService;
        private readonly IFileAccessService _fileAccessService;
        private readonly IApplicationLogService _applicationLogService;
        private readonly IUserRepository _userRepository;

        public UserService(
            IStorageService storageService,
            IFileAccessService fileAccessService,
            IApplicationLogService applicationLogService,
            IUserRepository userRepository)
        {
            _storageService = storageService;
            _fileAccessService = fileAccessService;
            _applicationLogService = applicationLogService;
            _userRepository = userRepository;
        }

        public async Task<bool> ExistsAsync(Guid userId)
        {
            return await _userRepository.ExistsAsync(userId).ConfigureAwait(false);
        }

        public async Task<bool> ExistsAsync(Guid userId, string email)
        {
            return await _userRepository.ExistsAsync(userId, email).ConfigureAwait(false);
        }

        public async Task AddAsync(User user)
        {
            await _userRepository.AddAsync(user).ConfigureAwait(false);
        }

        public async Task UpdateAsync(User user)
        {
            await _userRepository.UpdateAsync(user).ConfigureAwait(false);
        }

        public async Task<User> GetAsync(Guid userId)
        {
            return await _userRepository.GetAsync(userId).ConfigureAwait(false);
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _userRepository.GetAllAsync().ConfigureAwait(false);
        }

        public async Task DeleteAsync(Guid userId)
        {
            await _applicationLogService.InfoAsync($"Start deleting user with ID = '{userId}'.").ConfigureAwait(false);

            var directoryPath = _fileAccessService.GetRootPath(userId);
            if (Directory.Exists(directoryPath))
                Directory.Delete(directoryPath, true);

            await _storageService.DeleteContainerAsync(userId).ConfigureAwait(false);
            await _userRepository.DeleteAsync(userId).ConfigureAwait(false);

            await _applicationLogService.InfoAsync($"User with ID = '{userId}' was successfully deleted.").ConfigureAwait(false);
        }
    }
}
