using RewriteMe.DataAccess.Entities;
using RewriteMe.Domain.Transcription;

namespace RewriteMe.DataAccess.DataAdapters
{
    public static class UploadedChunkDataAdapter
    {
        public static UploadedChunk ToUploadedChunk(this UploadedChunkEntity uploadedChunkEntity)
        {
            return new UploadedChunk
            {
                Id = uploadedChunkEntity.Id,
                FileItemId = uploadedChunkEntity.FileItemId,
                Source = uploadedChunkEntity.Source,
                Order = uploadedChunkEntity.Order,
                DateCreatedUtc = uploadedChunkEntity.DateCreatedUtc
            };
        }

        public static UploadedChunkEntity ToUploadedChunkEntity(this UploadedChunk uploadedChunk)
        {
            return new UploadedChunkEntity
            {
                Id = uploadedChunk.Id,
                FileItemId = uploadedChunk.FileItemId,
                Source = uploadedChunk.Source,
                Order = uploadedChunk.Order,
                DateCreatedUtc = uploadedChunk.DateCreatedUtc
            };
        }
    }
}
