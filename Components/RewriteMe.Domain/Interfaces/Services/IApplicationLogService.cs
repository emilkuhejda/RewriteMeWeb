using System;
using System.Threading.Tasks;

namespace RewriteMe.Domain.Interfaces.Services
{
    public interface IApplicationLogService
    {
        Task DebugAsync(string message, Guid? userId = null);

        Task TraceAsync(string message, Guid? userId = null);

        Task InfoAsync(string message, Guid? userId = null);

        Task WarningAsync(string message, Guid? userId = null);

        Task ErrorAsync(string message, Guid? userId = null);

        Task CriticalAsync(string message, Guid? userId = null);
    }
}
