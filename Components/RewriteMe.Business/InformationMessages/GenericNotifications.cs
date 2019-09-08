using System;
using System.Collections.Generic;
using RewriteMe.Domain.Enums;
using RewriteMe.Domain.Messages;

namespace RewriteMe.Business.InformationMessages
{
    public static class GenericNotifications
    {
        public static InformationMessage GetTranscriptionSuccess(Guid userId, Guid fileItemId)
        {
            var informationMessageId = Guid.NewGuid();

            return new InformationMessage
            {
                Id = informationMessageId,
                UserId = userId,
                CampaignName = $"File transcription: {fileItemId}",
                DateCreated = DateTime.UtcNow,
                LanguageVersions = new List<LanguageVersion>
                {
                    new LanguageVersion
                    {
                        Id = Guid.NewGuid(),
                        InformationMessageId = informationMessageId,
                        Title = "RewriteMe finished task",
                        Message = "Your file was transcripted",
                        Description = "Thanks God, your file was transcripted.",
                        Language = Language.English
                    },
                    new LanguageVersion
                    {
                        Id = Guid.NewGuid(),
                        InformationMessageId = informationMessageId,
                        Title = "RewriteMe finished task",
                        Message = "Your file was transcripted",
                        Description = "Thanks God, your file was transcripted.",
                        Language = Language.Slovak
                    }
                }
            };
        }
    }
}
