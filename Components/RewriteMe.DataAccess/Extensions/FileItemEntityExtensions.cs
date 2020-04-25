using System;
using RewriteMe.DataAccess.Entities;

namespace RewriteMe.DataAccess.Extensions
{
    public static class FileItemEntityExtensions
    {
        public static FileItemEntity CreateDeletedEntity(this FileItemEntity fileItemEntity, Guid applicationId)
        {
            return new FileItemEntity
            {
                Id = fileItemEntity.Id,
                UserId = fileItemEntity.UserId,
                ApplicationId = applicationId,
                Name = "--DELETED--",
                FileName = "--DELETED--",
                Language = string.Empty,
                DateCreated = fileItemEntity.DateCreated,
                DateUpdatedUtc = DateTime.UtcNow,
                IsDeleted = true,
                IsPermanentlyDeleted = true,
                WasCleaned = true
            };
        }
    }
}
