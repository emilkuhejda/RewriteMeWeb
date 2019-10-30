using System;

namespace RewriteMe.DataAccess.Entities
{
    public class TranscribeItemSourceEntity
    {
        public Guid Id { get; set; }

        public Guid FileItemId { get; set; }

        public byte[] Source { get; set; }

        public DateTime DateCreatedUtc { get; set; }

        public virtual FileItemEntity FileItem { get; set; }
    }
}
