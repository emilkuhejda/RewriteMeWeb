using System.Linq;
using RewriteMe.Domain.Messages;
using RewriteMe.WebApi.Dtos;

namespace RewriteMe.WebApi.Extensions
{
    public static class InformationMessageExtensions
    {
        public static InformationMessageDto ToDto(this InformationMessage informationMessage)
        {
            return new InformationMessageDto
            {
                Id = informationMessage.Id,
                IsUserSpecific = informationMessage.UserId.HasValue,
                WasOpened = informationMessage.WasOpened,
                DateUpdatedUtc = informationMessage.DateUpdated,
                DatePublishedUtc = informationMessage.DatePublished.GetValueOrDefault(),
                LanguageVersions = informationMessage.LanguageVersions?.Select(x => x.ToDto())
            };
        }
    }
}
