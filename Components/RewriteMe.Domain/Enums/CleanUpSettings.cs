using System;

namespace RewriteMe.Domain.Enums
{
    [Flags]
    public enum CleanUpSettings
    {
        None = 0,
        Disk = 1,
        Database = 2
    }
}
