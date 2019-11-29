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
                Storage = fileItemEntity.Storage,
                TotalTime = fileItemEntity.TotalTime,
                TranscribedTime = fileItemEntity.TranscribedTime,
                DateCreated = fileItemEntity.DateCreated,
                DateProcessedUtc = fileItemEntity.DateProcessedUtc,
                DateUpdatedUtc = fileItemEntity.DateUpdatedUtc,
                IsDeleted = fileItemEntity.IsDeleted,
                IsPermanentlyDeleted = fileItemEntity.IsPermanentlyDeleted,
                WasCleaned = fileItemEntity.WasCleaned
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
                Storage = fileItem.Storage,
                TranscribedTime = fileItem.TranscribedTime,
                DateCreated = fileItem.DateCreated,
                DateProcessedUtc = fileItem.DateProcessedUtc,
                DateUpdatedUtc = fileItem.DateUpdatedUtc,
                IsDeleted = fileItem.IsDeleted,
                IsPermanentlyDeleted = fileItem.IsPermanentlyDeleted,
                WasCleaned = fileItem.WasCleaned
            };
        }
    }
}
