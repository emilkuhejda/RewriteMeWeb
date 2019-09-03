using System;
using System.Threading.Tasks;

namespace RewriteMe.Domain.Interfaces.Repositories
{
    public interface ILanguageVersionRepository
    {
        Task UpdateAndroidSendStatus(Guid languageVersionId, bool status);

        Task UpdateOsxSendStatus(Guid languageVersionId, bool status);
    }
}
