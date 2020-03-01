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
        private readonly IFileAccessService _fileAccessService;
        private readonly IApplicationLogService _applicationLogService;
        private readonly IUserRepository _userRepository;
        private readonly IDeletedAccountRepository _deletedAccountRepository;

        public UserService(
            IFileAccessService fileAccessService,
            IApplicationLogService applicationLogService,
            IUserRepository userRepository,
            IDeletedAccountRepository deletedAccountRepository)
        {
            _fileAccessService = fileAccessService;
            _applicationLogService = applicationLogService;
            _userRepository = userRepository;
            _deletedAccountRepository = deletedAccountRepository;
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

        public async Task<bool> DeleteAsync(Guid userId)
        {
            var directoryPath = _fileAccessService.GetRootPath(userId);
            if (Directory.Exists(directoryPath))
                Directory.Delete(directoryPath, true);

            await _userRepository.DeleteAsync(userId).ConfigureAwait(false);

            var deletedAccount = new DeletedAccount
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                DateDeleted = DateTime.UtcNow
            };
            await _deletedAccountRepository.AddAsync(deletedAccount).ConfigureAwait(false);

            await _applicationLogService.InfoAsync($"User with ID = '{userId}' was successfully deleted.").ConfigureAwait(false);
            return true;
        }
    }
}
