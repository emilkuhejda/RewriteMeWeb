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
                Language = fileItemEntity.Language,
                RecognitionState = fileItemEntity.RecognitionState,
                DateCreated = fileItemEntity.DateCreated,
                DateProcessed = fileItemEntity.DateProcessed
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
                Language = fileItem.Language,
                RecognitionState = fileItem.RecognitionState,
                DateCreated = fileItem.DateCreated,
                DateProcessed = fileItem.DateProcessed
            };
        }
    }
}
