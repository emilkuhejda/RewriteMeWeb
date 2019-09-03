using System;
using System.Threading.Tasks;
using RewriteMe.Domain.Enums;
using RewriteMe.Domain.Interfaces.Repositories;
using RewriteMe.Domain.Interfaces.Services;

namespace RewriteMe.Business.Services
{
    public class LanguageVersionService : ILanguageVersionService
    {
        private readonly ILanguageVersionRepository _languageVersionRepository;

        public LanguageVersionService(ILanguageVersionRepository languageVersionRepository)
        {
            _languageVersionRepository = languageVersionRepository;
        }

        public async Task UpdateSendStatusAsync(Guid languageVersionId, RuntimePlatform runtimePlatform, bool status)
        {
            switch (runtimePlatform)
            {
                case RuntimePlatform.Android:
                    await _languageVersionRepository.UpdateAndroidSendStatus(languageVersionId, status).ConfigureAwait(false);
                    break;
                case RuntimePlatform.Osx:
                    await _languageVersionRepository.UpdateOsxSendStatus(languageVersionId, status).ConfigureAwait(false);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(runtimePlatform));
            }
        }
    }
}
