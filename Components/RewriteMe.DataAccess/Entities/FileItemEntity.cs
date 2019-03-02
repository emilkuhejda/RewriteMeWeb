using System;

namespace RewriteMe.DataAccess.Entities
{
    public class FileItemEntity
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public string Name { get; set; }

        public string FileName { get; set; }

        public byte[] Source { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime? DateProcessed { get; set; }

        public UserEntity User { get; set; }
    }
}
