using System;
using System.Threading.Tasks;
using RewriteMe.Domain.Enums;
using RewriteMe.Domain.Interfaces.Repositories;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.Domain.Messages;
using Serilog;

namespace RewriteMe.Business.Services
{
    public class LanguageVersionService : ILanguageVersionService
    {
        private readonly ILanguageVersionRepository _languageVersionRepository;
        private readonly ILogger _logger;

        public LanguageVersionService(
            ILanguageVersionRepository languageVersionRepository,
            ILogger logger)
        {
            _languageVersionRepository = languageVersionRepository;
            _logger = logger.ForContext<LanguageVersionService>();
        }

        public async Task UpdateSendStatusAsync(LanguageVersion languageVersion, RuntimePlatform runtimePlatform, bool status)
        {
            _logger.Information($"Update send status for language version '{languageVersion}' on platform '{runtimePlatform}' to '{status}'.");

            switch (runtimePlatform)
            {
                case RuntimePlatform.Android:
                    await _languageVersionRepository.UpdateAndroidSendStatusAsync(languageVersion.Id, true).ConfigureAwait(false);
                    break;
                case RuntimePlatform.Osx:
                    await _languageVersionRepository.UpdateOsxSendStatusAsync(languageVersion.Id, true).ConfigureAwait(false);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(runtimePlatform));
            }
        }
    }
}
