using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RewriteMe.Domain.Enums;
using RewriteMe.Domain.Interfaces.Repositories;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.Domain.Notifications;
using Serilog;

namespace RewriteMe.Business.Services
{
    public class UserDeviceService : IUserDeviceService
    {
        private readonly IUserDeviceRepository _userDeviceRepository;
        private readonly ILogger _logger;

        public UserDeviceService(
            IUserDeviceRepository userDeviceRepository,
            ILogger logger)
        {
            _userDeviceRepository = userDeviceRepository;
            _logger = logger.ForContext<UserDeviceService>();
        }

        public async Task AddOrUpdateAsync(UserDevice userDevice)
        {
            await _userDeviceRepository.AddOrUpdateAsync(userDevice).ConfigureAwait(false);

            _logger.Information($"User device was updated. Device = {userDevice}.");
        }

        public async Task UpdateLanguageAsync(Guid userId, Guid installationId, Language language)
        {
            await _userDeviceRepository.UpdateLanguageAsync(userId, installationId, language).ConfigureAwait(false);

            _logger.Information($"Language for installation ID = '{installationId}' was changed to '{language}'.");
        }

        public async Task<IEnumerable<Guid>> GetPlatformSpecificInstallationIdsAsync(RuntimePlatform runtimePlatform, Language language, Guid? userId)
        {
            return await _userDeviceRepository.GetPlatformSpecificInstallationIdsAsync(runtimePlatform, language, userId).ConfigureAwait(false);
        }
    }
}
