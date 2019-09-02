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
                CampaignName = entity.CampaignName,
                Title = entity.Title,
                Message = entity.Message,
                Description = entity.Description,
                Language = entity.Language,
                SentOnOsx = entity.SentOnOsx,
                SentOnAndroid = entity.SentOnAndroid,
                DateCreated = entity.DateCreated
            };
        }

        public static InformationMessageEntity ToInformationMessageEntity(this InformationMessage informationMessage)
        {
            return new InformationMessageEntity
            {
                Id = informationMessage.Id,
                CampaignName = informationMessage.CampaignName,
                Title = informationMessage.Title,
                Message = informationMessage.Message,
                Description = informationMessage.Description,
                Language = informationMessage.Language,
                SentOnOsx = informationMessage.SentOnOsx,
                SentOnAndroid = informationMessage.SentOnAndroid,
                DateCreated = informationMessage.DateCreated
            };
        }
    }
}
