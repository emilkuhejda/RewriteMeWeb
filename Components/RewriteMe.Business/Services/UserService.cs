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
        private readonly IFileItemRepository _fileItemRepository;
        private readonly ITranscribeItemRepository _transcribeItemRepository;

        public UserService(
            IStorageService storageService,
            IFileAccessService fileAccessService,
            IApplicationLogService applicationLogService,
            IUserRepository userRepository,
            IFileItemRepository fileItemRepository,
            ITranscribeItemRepository transcribeItemRepository)
        {
            _storageService = storageService;
            _fileAccessService = fileAccessService;
            _applicationLogService = applicationLogService;
            _userRepository = userRepository;
            _fileItemRepository = fileItemRepository;
            _transcribeItemRepository = transcribeItemRepository;
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

            var fileItems = await _fileItemRepository.GetAllForUserAsync(userId).ConfigureAwait(false);
            foreach (var fileItem in fileItems)
            {
                fileItem.TranscribeItems = await _transcribeItemRepository.GetAllAsync(fileItem.Id).ConfigureAwait(false);
                await _storageService.DeleteFileItemAsync(fileItem).ConfigureAwait(false);
            }

            await _userRepository.DeleteAsync(userId).ConfigureAwait(false);

            await _applicationLogService.InfoAsync($"User with ID = '{userId}' was successfully deleted.").ConfigureAwait(false);
        }
    }
}
