using System;
using System.Threading.Tasks;
using RewriteMe.Domain.Enums;

namespace RewriteMe.Domain.Interfaces.Services
{
    public interface ICleanUpService
    {
        void CleanUp(DateTime deleteBefore, CleanUpSettings cleanUpSettings, bool forceCleanUp);

        Task CleanUpAsync(DateTime deleteBefore, CleanUpSettings cleanUpSettings, bool forceCleanUp);
    }
}
