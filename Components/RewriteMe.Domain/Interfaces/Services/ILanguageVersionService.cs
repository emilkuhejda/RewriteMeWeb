using System;
using System.Threading.Tasks;
using RewriteMe.Domain.Enums;

namespace RewriteMe.Domain.Interfaces.Services
{
    public interface ILanguageVersionService
    {
        Task UpdateSendStatusAsync(Guid languageVersionId, RuntimePlatform runtimePlatform, bool status);
    }
}
