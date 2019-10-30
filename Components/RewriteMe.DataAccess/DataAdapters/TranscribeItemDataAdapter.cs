using System.Collections.Generic;
using Newtonsoft.Json;
using RewriteMe.DataAccess.Entities;
using RewriteMe.Domain.Transcription;

namespace RewriteMe.DataAccess.DataAdapters
{
    public static class TranscribeItemDataAdapter
    {
        public static TranscribeItem ToTranscribeItem(this TranscribeItemEntity transcribeItemEntity)
        {
            return new TranscribeItem
            {
                Id = transcribeItemEntity.Id,
                FileItemId = transcribeItemEntity.FileItemId,
                ApplicationId = transcribeItemEntity.ApplicationId,
                Alternatives = JsonConvert.DeserializeObject<IEnumerable<RecognitionAlternative>>(transcribeItemEntity.Alternatives),
                UserTranscript = transcribeItemEntity.UserTranscript,
                SourceFileName = transcribeItemEntity.SourceFileName,
                StartTime = transcribeItemEntity.StartTime,
                EndTime = transcribeItemEntity.EndTime,
                TotalTime = transcribeItemEntity.TotalTime,
                DateCreatedUtc = transcribeItemEntity.DateCreatedUtc,
                DateUpdatedUtc = transcribeItemEntity.DateUpdatedUtc
            };
        }

        public static TranscribeItemEntity ToTranscribeItemEntity(this TranscribeItem transcribeItem)
        {
            return new TranscribeItemEntity
            {
                Id = transcribeItem.Id,
                FileItemId = transcribeItem.FileItemId,
                ApplicationId = transcribeItem.ApplicationId,
                Alternatives = JsonConvert.SerializeObject(transcribeItem.Alternatives),
                UserTranscript = transcribeItem.UserTranscript,
                SourceFileName = transcribeItem.SourceFileName,
                StartTime = transcribeItem.StartTime,
                EndTime = transcribeItem.EndTime,
                TotalTime = transcribeItem.TotalTime,
                DateCreatedUtc = transcribeItem.DateCreatedUtc,
                DateUpdatedUtc = transcribeItem.DateUpdatedUtc
            };
        }
    }
}
