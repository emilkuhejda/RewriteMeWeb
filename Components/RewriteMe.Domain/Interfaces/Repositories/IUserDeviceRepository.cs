using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RewriteMe.Domain.Enums;
using RewriteMe.Domain.Notifications;

namespace RewriteMe.Domain.Interfaces.Repositories
{
    public interface IUserDeviceRepository
    {
        Task AddOrUpdateAsync(UserDevice userDevice);

        Task UpdateLanguageAsync(Guid userId, Guid installationId, Language language);

        Task<IEnumerable<Guid>> GetPlatformSpecificInstallationIdsAsync(RuntimePlatform runtimePlatform, Language language, Guid? userId);
    }
}
