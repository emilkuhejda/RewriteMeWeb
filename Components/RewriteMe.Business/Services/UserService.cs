﻿using System;
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
        private readonly IUserRepository _userRepository;

        public UserService(
            IFileAccessService fileAccessService,
            IUserRepository userRepository)
        {
            _fileAccessService = fileAccessService;
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

        public async Task<bool> DeleteAsync(Guid userId)
        {
            var path = _fileAccessService.GetRootPath(userId);
            if (Directory.Exists(path))
                Directory.Delete(path, true);

            await _userRepository.DeleteAsync(userId).ConfigureAwait(false);
            return true;
        }
    }
}
