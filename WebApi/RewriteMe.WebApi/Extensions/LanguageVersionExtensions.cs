using RewriteMe.Domain.Dtos;
using RewriteMe.Domain.Messages;

namespace RewriteMe.WebApi.Extensions
{
    public static class LanguageVersionExtensions
    {
        public static LanguageVersionDto ToDto(this LanguageVersion languageVersion)
        {
            return new LanguageVersionDto
            {
                Id = languageVersion.Id,
                InformationMessageId = languageVersion.InformationMessageId,
                Title = languageVersion.Title,
                Message = languageVersion.Message,
                Description = languageVersion.Description,
                LanguageString = languageVersion.Language.ToString(),
                SentOnOsx = languageVersion.SentOnOsx,
                SentOnAndroid = languageVersion.SentOnAndroid
            };
        }
    }
}
