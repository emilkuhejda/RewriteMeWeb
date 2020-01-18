using System;

namespace RewriteMe.DataAccess.Entities
{
    public class UploadedChunkEntity
    {
        public Guid Id { get; set; }

        public Guid FileItemId { get; set; }

        public Guid ApplicationId { get; set; }

        public byte[] Source { get; set; }

        public int Order { get; set; }

        public DateTime DateCreatedUtc { get; set; }
    }
}
