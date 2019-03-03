using System.Collections.Generic;
using Newtonsoft.Json;
using RewriteMe.DataAccess.Entities;
using RewriteMe.Domain.Transcription;

namespace RewriteMe.DataAccess.DataAdapters
{
    public static class TranscribeItemEntityDataAdapter
    {
        public static TranscribeItem ToTranscribeItem(this TranscribeItemEntity transcribeItemEntity)
        {
            return new TranscribeItem
            {
                Id = transcribeItemEntity.Id,
                FileItemId = transcribeItemEntity.FileItemId,
                Alternatives = JsonConvert.DeserializeObject<IEnumerable<RecognitionAlternative>>(transcribeItemEntity.Alternatives),
                Source = transcribeItemEntity.Source,
                TotalTime = transcribeItemEntity.TotalTime,
                DateCreated = transcribeItemEntity.DateCreated
            };
        }

        public static TranscribeItemEntity ToTranscribeItemEntity(this TranscribeItem transcribeItem)
        {
            return new TranscribeItemEntity
            {
                Id = transcribeItem.Id,
                FileItemId = transcribeItem.FileItemId,
                Alternatives = JsonConvert.SerializeObject(transcribeItem.Alternatives),
                Source = transcribeItem.Source,
                TotalTime = transcribeItem.TotalTime,
                DateCreated = transcribeItem.DateCreated
            };
        }
    }
}
