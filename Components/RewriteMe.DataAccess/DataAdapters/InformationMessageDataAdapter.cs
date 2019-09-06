using System.Linq;
using RewriteMe.DataAccess.Entities;
using RewriteMe.Domain.Messages;

namespace RewriteMe.DataAccess.DataAdapters
{
    public static class InformationMessageDataAdapter
    {
        public static InformationMessage ToInformationMessage(this InformationMessageEntity entity)
        {
            return new InformationMessage
            {
                Id = entity.Id,
                UserId = entity.UserId,
                CampaignName = entity.CampaignName,
                DateCreated = entity.DateCreated,
                LanguageVersions = entity.LanguageVersions?.Select(x => x.ToLanguageVersion()).ToList()
            };
        }

        public static InformationMessageEntity ToInformationMessageEntity(this InformationMessage informationMessage)
        {
            return new InformationMessageEntity
            {
                Id = informationMessage.Id,
                UserId = informationMessage.UserId,
                CampaignName = informationMessage.CampaignName,
                DateCreated = informationMessage.DateCreated,
                LanguageVersions = informationMessage.LanguageVersions?.Select(x => x.ToLanguageVersionEntity()).ToList()
            };
        }
    }
}
