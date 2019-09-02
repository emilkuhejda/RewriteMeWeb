using RewriteMe.DataAccess.Entities;
using RewriteMe.Domain.Messages;

namespace RewriteMe.DataAccess.DataAdapters
{
    public static class LanguageVersionDataAdapter
    {
        public static LanguageVersion ToLanguageVersion(this LanguageVersionEntity entity)
        {
            return new LanguageVersion
            {
                Id = entity.Id,
                InformationMessageId = entity.InformationMessageId,
                Title = entity.Title,
                Message = entity.Message,
                Description = entity.Description,
                Language = entity.Language
            };
        }

        public static LanguageVersionEntity ToLanguageVersionEntity(this LanguageVersion languageVersion)
        {
            return new LanguageVersionEntity
            {
                Id = languageVersion.Id,
                InformationMessageId = languageVersion.InformationMessageId,
                Title = languageVersion.Title,
                Message = languageVersion.Message,
                Description = languageVersion.Description,
                Language = languageVersion.Language
            };
        }
    }
}
