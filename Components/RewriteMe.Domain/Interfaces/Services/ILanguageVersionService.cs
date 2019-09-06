using System.Threading.Tasks;
using RewriteMe.Domain.Enums;
using RewriteMe.Domain.Messages;

namespace RewriteMe.Domain.Interfaces.Services
{
    public interface ILanguageVersionService
    {
        Task UpdateSendStatusAsync(LanguageVersion languageVersion, RuntimePlatform runtimePlatform, bool status);
    }
}
