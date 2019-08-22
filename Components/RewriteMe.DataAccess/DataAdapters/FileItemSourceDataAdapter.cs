using RewriteMe.DataAccess.Entities;
using RewriteMe.Domain.Transcription;

namespace RewriteMe.DataAccess.DataAdapters
{
    public static class FileItemSourceDataAdapter
    {
        public static FileItemSource ToFileItemSource(this FileItemSourceEntity entity)
        {
            return new FileItemSource
            {
                Id = entity.Id,
                FileItemId = entity.FileItemId,
                Source = entity.Source,
                DateCreated = entity.DateCreated
            };
        }

        public static FileItemSourceEntity ToFileItemSourceEntity(this FileItemSource fileItemSource)
        {
            return new FileItemSourceEntity
            {
                Id = fileItemSource.Id,
                FileItemId = fileItemSource.FileItemId,
                Source = fileItemSource.Source,
                DateCreated = fileItemSource.DateCreated
            };
        }
    }
}
