using System;
using RewriteMe.Domain.Enums;
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
                RuntimePlatform = GetRuntimePlatform(model.RuntimePlatform),
                InstalledVersionNumber = model.InstalledVersionNumber,
                Language = GetLanguage(model.Language),
                DateRegisteredUtc = DateTime.UtcNow
            };
        }

        private static RuntimePlatform GetRuntimePlatform(string runtimePlatform)
        {
            switch (runtimePlatform)
            {
                case "Android":
                    return RuntimePlatform.Android;
                case "iOS":
                    return RuntimePlatform.Osx;
                default:
                    return RuntimePlatform.Undefined;
            }
        }

        private static Language GetLanguage(string language)
        {
            switch (language)
            {
                case "en":
                    return Language.English;
                case "sk":
                    return Language.Slovak;
                default:
                    return Language.Undefined;
            }
        }
    }
}
