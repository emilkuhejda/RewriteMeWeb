using System;
using RewriteMe.Domain.Enums;
using RewriteMe.Domain.Messages;
using RewriteMe.WebApi.Models;

namespace RewriteMe.WebApi.Extensions
{
    public static class CreateInformationMessageModelExtensions
    {
        public static InformationMessage ToInformationMessage(this CreateInformationMessageModel model)
        {
            var languageVersions = new[]
            {
                GetLanguageVersion(model, Language.English),
                GetLanguageVersion(model, Language.Slovak)
            };

            return new InformationMessage
            {
                Id = Guid.NewGuid(),
                CampaignName = model.CampaignName,
                DateCreated = DateTime.UtcNow,
                LanguageVersions = languageVersions
            };
        }

        private static LanguageVersion GetLanguageVersion(this CreateInformationMessageModel model, Language language)
        {
            return new LanguageVersion
            {
                Title = model.GetTitle(language),
                Message = model.GetMessage(language),
                Description = model.GetDescription(language),
                Language = language,
            };
        }

        private static string GetTitle(this CreateInformationMessageModel model, Language language)
        {
            switch (language)
            {
                case Language.English:
                    return model.TitleEn;
                case Language.Slovak:
                    return model.TitleSk;
                default:
                    throw new ArgumentOutOfRangeException(nameof(language));
            }
        }

        private static string GetMessage(this CreateInformationMessageModel model, Language language)
        {
            switch (language)
            {
                case Language.English:
                    return model.MessageEn;
                case Language.Slovak:
                    return model.MessageSk;
                default:
                    throw new ArgumentOutOfRangeException(nameof(language));
            }
        }

        private static string GetDescription(this CreateInformationMessageModel model, Language language)
        {
            switch (language)
            {
                case Language.English:
                    return model.DescriptionEn;
                case Language.Slovak:
                    return model.DescriptionSk;
                default:
                    throw new ArgumentOutOfRangeException(nameof(language));
            }
        }
    }
}
