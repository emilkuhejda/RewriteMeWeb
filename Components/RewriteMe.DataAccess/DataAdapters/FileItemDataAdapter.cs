using RewriteMe.DataAccess.Entities;
using RewriteMe.Domain.Transcription;

namespace RewriteMe.DataAccess.DataAdapters
{
    public static class FileItemDataAdapter
    {
        public static FileItem ToFileItem(this FileItemEntity fileItemEntity)
        {
            return new FileItem
            {
                Id = fileItemEntity.Id,
                UserId = fileItemEntity.UserId,
                ApplicationId = fileItemEntity.ApplicationId,
                Name = fileItemEntity.Name,
                FileName = fileItemEntity.FileName,
                Language = fileItemEntity.Language,
                RecognitionState = fileItemEntity.RecognitionState,
                OriginalSourceFileName = fileItemEntity.OriginalSourceFileName,
                SourceFileName = fileItemEntity.SourceFileName,
                OriginalContentType = fileItemEntity.OriginalContentType,
                TotalTime = fileItemEntity.TotalTime,
                DateCreated = fileItemEntity.DateCreated,
                DateProcessed = fileItemEntity.DateProcessed,
                DateUpdated = fileItemEntity.DateUpdated,
                IsDeleted = fileItemEntity.IsDeleted
            };
        }

        public static FileItemEntity ToFileItemEntity(this FileItem fileItem)
        {
            return new FileItemEntity
            {
                Id = fileItem.Id,
                UserId = fileItem.UserId,
                ApplicationId = fileItem.ApplicationId,
                Name = fileItem.Name,
                FileName = fileItem.FileName,
                Language = fileItem.Language,
                RecognitionState = fileItem.RecognitionState,
                OriginalSourceFileName = fileItem.OriginalSourceFileName,
                SourceFileName = fileItem.SourceFileName,
                OriginalContentType = fileItem.OriginalContentType,
                TotalTime = fileItem.TotalTime,
                DateCreated = fileItem.DateCreated,
                DateProcessed = fileItem.DateProcessed,
                DateUpdated = fileItem.DateUpdated,
                IsDeleted = fileItem.IsDeleted
            };
        }
    }
}
