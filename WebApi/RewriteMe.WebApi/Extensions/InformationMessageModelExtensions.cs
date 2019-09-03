using System;
using RewriteMe.Domain.Enums;
using RewriteMe.Domain.Messages;
using RewriteMe.WebApi.Models;

namespace RewriteMe.WebApi.Extensions
{
    public static class InformationMessageModelExtensions
    {
        public static InformationMessage ToInformationMessage(this InformationMessageModel model, Guid informationMessageId)
        {
            var languageVersions = new[]
            {
                GetLanguageVersion(model, informationMessageId, Language.English),
                GetLanguageVersion(model, informationMessageId, Language.Slovak)
            };

            return new InformationMessage
            {
                Id = informationMessageId,
                CampaignName = model.CampaignName,
                DateCreated = DateTime.UtcNow,
                LanguageVersions = languageVersions
            };
        }

        private static LanguageVersion GetLanguageVersion(this InformationMessageModel model, Guid informationMessageId, Language language)
        {
            return new LanguageVersion
            {
                Id = Guid.NewGuid(),
                InformationMessageId = informationMessageId,
                Title = model.GetTitle(language),
                Message = model.GetMessage(language),
                Description = model.GetDescription(language),
                Language = language,
            };
        }

        private static string GetTitle(this InformationMessageModel model, Language language)
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

        private static string GetMessage(this InformationMessageModel model, Language language)
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

        private static string GetDescription(this InformationMessageModel model, Language language)
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
