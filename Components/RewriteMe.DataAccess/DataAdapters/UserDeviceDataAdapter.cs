using RewriteMe.DataAccess.Entities;
using RewriteMe.Domain.Notifications;

namespace RewriteMe.DataAccess.DataAdapters
{
    public static class UserDeviceDataAdapter
    {
        public static UserDevice ToUserDevice(this UserDeviceEntity entity)
        {
            return new UserDevice
            {
                Id = entity.Id,
                UserId = entity.UserId,
                InstallationId = entity.InstallationId,
                RuntimePlatform = entity.RuntimePlatform,
                InstalledVersionNumber = entity.InstalledVersionNumber,
                Language = entity.Language,
                DateRegisteredUtc = entity.DateRegisteredUtc
            };
        }

        public static UserDeviceEntity ToUserDeviceEntity(this UserDevice userDevice)
        {
            return new UserDeviceEntity
            {
                Id = userDevice.Id,
                UserId = userDevice.UserId,
                InstallationId = userDevice.InstallationId,
                RuntimePlatform = userDevice.RuntimePlatform,
                InstalledVersionNumber = userDevice.InstalledVersionNumber,
                Language = userDevice.Language,
                DateRegisteredUtc = userDevice.DateRegisteredUtc
            };
        }
    }
}
