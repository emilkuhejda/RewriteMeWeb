using System;

namespace RewriteMe.DataAccess.Entities
{
    public class FileItemSourceEntity
    {
        public Guid Id { get; set; }

        public Guid FileItemId { get; set; }

        public byte[] OriginalSource { get; set; }

        public byte[] Source { get; set; }

        public DateTime DateCreatedUtc { get; set; }

        public virtual FileItemEntity FileItem { get; set; }
    }
}
