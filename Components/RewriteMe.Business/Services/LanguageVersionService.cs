﻿using System;
using System.Threading.Tasks;
using RewriteMe.Domain.Enums;
using RewriteMe.Domain.Interfaces.Repositories;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.Domain.Messages;

namespace RewriteMe.Business.Services
{
    public class LanguageVersionService : ILanguageVersionService
    {
        private readonly ILanguageVersionRepository _languageVersionRepository;

        public LanguageVersionService(ILanguageVersionRepository languageVersionRepository)
        {
            _languageVersionRepository = languageVersionRepository;
        }

        public async Task UpdateSendStatusAsync(LanguageVersion languageVersion, RuntimePlatform runtimePlatform, bool status)
        {
            switch (runtimePlatform)
            {
                case RuntimePlatform.Android:
                    languageVersion.SentOnAndroid = true;
                    break;
                case RuntimePlatform.Osx:
                    languageVersion.SentOnOsx = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(runtimePlatform));
            }

            await _languageVersionRepository.UpdateAsync(languageVersion).ConfigureAwait(false);
        }
    }
}
