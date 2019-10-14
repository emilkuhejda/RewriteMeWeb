using System;
using System.Threading.Tasks;
using RewriteMe.Domain.Enums;

namespace RewriteMe.Domain.Interfaces.Services
{
    public interface ICleanUpService
    {
        Task CleanAsync(DateTime deleteBefore, CleanUpSettings cleanUpSettings);
    }
}
