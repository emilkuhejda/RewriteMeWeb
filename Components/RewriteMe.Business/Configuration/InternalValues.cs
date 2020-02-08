using RewriteMe.Domain.Configuration;
using RewriteMe.Domain.Enums;
using Storage = RewriteMe.Domain.Enums.StorageSetting;

namespace RewriteMe.Business.Configuration
{
    public static class InternalValues
    {
        public static InternalValue<StorageSetting> StorageSetting { get; } = new InternalValue<StorageSetting>("StorageSetting", Storage.Disk);

        public static InternalValue<StorageSetting> ChunksStorageSetting { get; } = new InternalValue<StorageSetting>("ChunksStorageSetting", Storage.Disk);

        public static InternalValue<bool> IsDatabaseBackupEnabled { get; } = new InternalValue<bool>("IsDatabaseBackupEnabled", false);

        public static InternalValue<bool> IsProgressNotificationsEnabled { get; } = new InternalValue<bool>("IsProgressNotificationsEnabled", false);
    }
}
