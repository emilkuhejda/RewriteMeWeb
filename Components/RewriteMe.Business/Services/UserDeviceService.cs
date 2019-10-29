using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RewriteMe.Domain.Enums;
using RewriteMe.Domain.Interfaces.Repositories;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.Domain.Notifications;

namespace RewriteMe.Business.Services
{
    public class UserDeviceService : IUserDeviceService
    {
        private readonly IUserDeviceRepository _userDeviceRepository;

        public UserDeviceService(IUserDeviceRepository userDeviceRepository)
        {
            _userDeviceRepository = userDeviceRepository;
        }

        public async Task AddOrUpdateAsync(UserDevice userDevice)
        {
            await _userDeviceRepository.AddOrUpdateAsync(userDevice).ConfigureAwait(false);
        }

        public async Task UpdateLanguageAsync(Guid userId, Guid installationId, Language language)
        {
            await _userDeviceRepository.UpdateLanguageAsync(userId, installationId, language).ConfigureAwait(false);
        }

        public async Task<IEnumerable<Guid>> GetPlatformSpecificInstallationIdsAsync(RuntimePlatform runtimePlatform, Language language, Guid? userId)
        {
            return await _userDeviceRepository.GetPlatformSpecificInstallationIdsAsync(runtimePlatform, language, userId).ConfigureAwait(false);
        }
    }
}
