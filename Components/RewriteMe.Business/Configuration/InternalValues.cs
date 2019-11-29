using RewriteMe.Domain.Configuration;
using RewriteMe.Domain.Enums;

namespace RewriteMe.Business.Configuration
{
    public static class InternalValues
    {
        public static InternalValue<StorageSetting> StorageSetting { get; } = new InternalValue<StorageSetting>("StorageSetting", Domain.Enums.StorageSetting.Disk);

        public static InternalValue<bool> IsDatabaseBackupEnabled { get; } = new InternalValue<bool>("IsDatabaseBackupEnabled", false);

        public static InternalValue<bool> IsProgressNotificationsEnabled { get; } = new InternalValue<bool>("IsProgressNotificationsEnabled", false);
    }
}
