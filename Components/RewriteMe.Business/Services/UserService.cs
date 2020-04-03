using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using RewriteMe.Domain.Interfaces.Repositories;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.Domain.UserManagement;
using Serilog;

namespace RewriteMe.Business.Services
{
    public class UserService : IUserService
    {
        private readonly IStorageService _storageService;
        private readonly IFileAccessService _fileAccessService;
        private readonly IUserRepository _userRepository;
        private readonly ILogger _logger;

        public UserService(
            IStorageService storageService,
            IFileAccessService fileAccessService,
            IUserRepository userRepository,
            ILogger logger)
        {
            _storageService = storageService;
            _fileAccessService = fileAccessService;
            _userRepository = userRepository;
            _logger = logger.ForContext<UserService>();
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

            _logger.Information($"User ID = '{user.Id}' was created.");
        }

        public async Task UpdateAsync(User user)
        {
            await _userRepository.UpdateAsync(user).ConfigureAwait(false);

            _logger.Information($"User ID = '{user.Id}' was updated.");
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
            _logger.Information($"Start deleting user with ID = '{userId}'.");

            var directoryPath = _fileAccessService.GetRootPath(userId);
            if (Directory.Exists(directoryPath))
                Directory.Delete(directoryPath, true);

            await _storageService.DeleteContainerAsync(userId).ConfigureAwait(false);
            await _userRepository.DeleteAsync(userId).ConfigureAwait(false);

            _logger.Information($"User with ID = '{userId}' was successfully deleted.");
        }
    }
}
