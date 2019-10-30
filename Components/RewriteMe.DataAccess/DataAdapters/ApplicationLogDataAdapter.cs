using RewriteMe.DataAccess.Entities;
using RewriteMe.Domain.Logging;

namespace RewriteMe.DataAccess.DataAdapters
{
    public static class ApplicationLogDataAdapter
    {
        public static ApplicationLog ToApplicationLog(this ApplicationLogEntity entity)
        {
            return new ApplicationLog
            {
                Id = entity.Id,
                UserId = entity.UserId,
                LogLevel = entity.LogLevel,
                Message = entity.Message,
                DateCreatedUtc = entity.DateCreatedUtc
            };
        }

        public static ApplicationLogEntity ToApplicationLogEntity(this ApplicationLog applicationLog)
        {
            return new ApplicationLogEntity
            {
                Id = applicationLog.Id,
                UserId = applicationLog.UserId,
                LogLevel = applicationLog.LogLevel,
                Message = applicationLog.Message,
                DateCreatedUtc = applicationLog.DateCreatedUtc
            };
        }
    }
}
