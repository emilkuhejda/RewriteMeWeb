using System.Linq;
using RewriteMe.Domain.Dtos;
using RewriteMe.Domain.Messages;

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
                DateUpdatedUtc = informationMessage.DateUpdatedUtc,
                DatePublishedUtc = informationMessage.DatePublishedUtc.GetValueOrDefault(),
                LanguageVersions = informationMessage.LanguageVersions?.Select(x => x.ToDto())
            };
        }
    }
}
