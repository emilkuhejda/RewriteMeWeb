using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RewriteMe.DataAccess.DataAdapters;
using RewriteMe.Domain.Enums;
using RewriteMe.Domain.Interfaces.Repositories;
using RewriteMe.Domain.Notifications;

namespace RewriteMe.DataAccess.Repositories
{
    public class UserDeviceRepository : IUserDeviceRepository
    {
        private readonly IDbContextFactory _contextFactory;

        public UserDeviceRepository(IDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task AddOrUpdateAsync(UserDevice userDevice)
        {
            using (var context = _contextFactory.Create())
            {
                var entity = await context.UserDevices
                    .SingleOrDefaultAsync(x => x.UserId == userDevice.UserId && x.InstallationId == userDevice.InstallationId)
                    .ConfigureAwait(false);

                if (entity == null)
                {
                    await context.UserDevices.AddAsync(userDevice.ToUserDeviceEntity()).ConfigureAwait(false);
                }
                else
                {
                    context.Entry(userDevice.ToUserDeviceEntity()).State = EntityState.Modified;
                }

                await context.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        public async Task<IEnumerable<Guid>> GetPlatformSpecificInstallationIdsAsync(RuntimePlatform runtimePlatform, Language language)
        {
            using (var context = _contextFactory.Create())
            {
                return await context.UserDevices
                    .AsNoTracking()
                    .Where(x => x.RuntimePlatform == runtimePlatform && x.Language == language)
                    .Select(x => x.InstallationId)
                    .ToListAsync()
                    .ConfigureAwait(false);
            }
        }
    }
}
