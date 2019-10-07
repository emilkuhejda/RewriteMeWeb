using System;
using System.Threading.Tasks;

namespace RewriteMe.Domain.Interfaces.Repositories
{
    public interface ILanguageVersionRepository
    {
        Task UpdateAndroidSendStatusAsync(Guid languageVersionId, bool status);

        Task UpdateOsxSendStatusAsync(Guid languageVersionId, bool status);
    }
}
