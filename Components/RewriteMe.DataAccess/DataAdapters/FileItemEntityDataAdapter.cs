using System.Linq;
using RewriteMe.DataAccess.Entities;
using RewriteMe.Domain.Transcription;

namespace RewriteMe.DataAccess.DataAdapters
{
    public static class FileItemEntityDataAdapter
    {
        public static FileItem ToFileItem(this FileItemEntity fileItemEntity)
        {
            return new FileItem
            {
                Id = fileItemEntity.Id,
                UserId = fileItemEntity.UserId,
                Name = fileItemEntity.Name,
                FileName = fileItemEntity.FileName,
                Source = fileItemEntity.Source,
                ContentType = fileItemEntity.ContentType,
                RecognitionState = fileItemEntity.RecognitionState,
                DateCreated = fileItemEntity.DateCreated,
                DateProcessed = fileItemEntity.DateProcessed,
                TranscribeItems = fileItemEntity.TranscribeItems.Select(x => x.ToTranscribeItem())
            };
        }

        public static FileItemEntity ToFileItemEntity(this FileItem fileItem)
        {
            return new FileItemEntity
            {
                Id = fileItem.Id,
                UserId = fileItem.UserId,
                Name = fileItem.Name,
                FileName = fileItem.FileName,
                Source = fileItem.Source,
                ContentType = fileItem.ContentType,
                RecognitionState = fileItem.RecognitionState,
                DateCreated = fileItem.DateCreated,
                DateProcessed = fileItem.DateProcessed,
                TranscribeItems = fileItem.TranscribeItems.Select(x => x.ToTranscribeItemEntity())
            };
        }
    }
}
