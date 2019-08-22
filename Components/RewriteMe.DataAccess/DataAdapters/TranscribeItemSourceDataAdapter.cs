using RewriteMe.DataAccess.Entities;
using RewriteMe.Domain.Transcription;

namespace RewriteMe.DataAccess.DataAdapters
{
    public static class TranscribeItemSourceDataAdapter
    {
        public static TranscribeItemSource ToTranscribeItemSource(this TranscribeItemSourceEntity entity)
        {
            return new TranscribeItemSource
            {
                Id = entity.Id,
                FileItemId = entity.FileItemId,
                Source = entity.Source,
                DateCreated = entity.DateCreated
            };
        }

        public static TranscribeItemSourceEntity ToTranscribeItemSourceEntity(this TranscribeItemSource transcribeItemSource)
        {
            return new TranscribeItemSourceEntity
            {
                Id = transcribeItemSource.Id,
                FileItemId = transcribeItemSource.FileItemId,
                Source = transcribeItemSource.Source,
                DateCreated = transcribeItemSource.DateCreated
            };
        }
    }
}
