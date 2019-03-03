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
                Transcript = transcribeItemEntity.Transcript,
                Source = transcribeItemEntity.Source,
                Duration = transcribeItemEntity.Duration
            };
        }

        public static TranscribeItemEntity ToTranscribeItemEntity(this TranscribeItem transcribeItem)
        {
            return new TranscribeItemEntity
            {
                Id = transcribeItem.Id,
                FileItemId = transcribeItem.FileItemId,
                Transcript = transcribeItem.Transcript,
                Source = transcribeItem.Source,
                Duration = transcribeItem.Duration
            };
        }
    }
}
