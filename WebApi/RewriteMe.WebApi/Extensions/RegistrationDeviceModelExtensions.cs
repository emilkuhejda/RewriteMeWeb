using System;
using RewriteMe.Domain.Notifications;
using RewriteMe.WebApi.Models;

namespace RewriteMe.WebApi.Extensions
{
    public static class RegistrationDeviceModelExtensions
    {
        public static UserDevice ToUserDevice(this RegistrationDeviceModel model, Guid userId)
        {
            return new UserDevice
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                InstallationId = model.InstallationId,
                RuntimePlatform = model.RuntimePlatform,
                InstalledVersionNumber = model.InstalledVersionNumber,
                Language = model.Language,
                DateRegistered = DateTime.UtcNow
            };
        }
    }
}
