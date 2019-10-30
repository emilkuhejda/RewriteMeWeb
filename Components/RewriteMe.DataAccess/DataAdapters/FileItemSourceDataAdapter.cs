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
                OriginalSource = entity.OriginalSource,
                Source = entity.Source,
                DateCreatedUtc = entity.DateCreatedUtc
            };
        }

        public static FileItemSourceEntity ToFileItemSourceEntity(this FileItemSource fileItemSource)
        {
            return new FileItemSourceEntity
            {
                Id = fileItemSource.Id,
                FileItemId = fileItemSource.FileItemId,
                OriginalSource = fileItemSource.OriginalSource,
                Source = fileItemSource.Source,
                DateCreatedUtc = fileItemSource.DateCreatedUtc
            };
        }
    }
}
