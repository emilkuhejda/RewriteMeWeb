using System;
using System.Threading.Tasks;
using RewriteMe.Domain.Enums;
using RewriteMe.Domain.Interfaces.Repositories;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.Domain.Logging;

namespace RewriteMe.Business.Services
{
    public class ApplicationLogService : IApplicationLogService
    {
        private readonly IApplicationLogRepository _applicationLogRepository;

        public ApplicationLogService(IApplicationLogRepository applicationLogRepository)
        {
            _applicationLogRepository = applicationLogRepository;
        }

        public async Task DebugAsync(string message, Guid? userId = null)
        {
            var log = CreateApplicationLog(message, userId, ApplicationLogLevel.Debug);

            await AddAsync(log).ConfigureAwait(false);
        }

        public async Task TraceAsync(string message, Guid? userId = null)
        {
            var log = CreateApplicationLog(message, userId, ApplicationLogLevel.Trace);

            await AddAsync(log).ConfigureAwait(false);
        }

        public async Task InfoAsync(string message, Guid? userId = null)
        {
            var log = CreateApplicationLog(message, userId, ApplicationLogLevel.Info);

            await AddAsync(log).ConfigureAwait(false);
        }

        public async Task WarningAsync(string message, Guid? userId = null)
        {
            var log = CreateApplicationLog(message, userId, ApplicationLogLevel.Warning);

            await AddAsync(log).ConfigureAwait(false);
        }

        public async Task ErrorAsync(string message, Guid? userId = null)
        {
            var log = CreateApplicationLog(message, userId, ApplicationLogLevel.Error);

            await AddAsync(log).ConfigureAwait(false);
        }

        public async Task CriticalAsync(string message, Guid? userId = null)
        {
            var log = CreateApplicationLog(message, userId, ApplicationLogLevel.Critical);

            await AddAsync(log).ConfigureAwait(false);
        }

        private async Task AddAsync(ApplicationLog applicationLog)
        {
            await _applicationLogRepository.AddAsync(applicationLog).ConfigureAwait(false);
        }

        private ApplicationLog CreateApplicationLog(string message, Guid? userId, ApplicationLogLevel logLevel)
        {
            return new ApplicationLog
            {
                UserId = userId,
                LogLevel = logLevel,
                Message = message,
                DateCreated = DateTime.UtcNow
            };
        }
    }
}
