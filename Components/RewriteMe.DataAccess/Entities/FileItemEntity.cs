using System;
using System.Collections.Generic;

namespace RewriteMe.DataAccess.Entities
{
    public class FileItemEntity
    {
        public FileItemEntity()
        {
            TranscribeItems = new HashSet<TranscribeItemEntity>();
        }

        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public string Name { get; set; }

        public string FileName { get; set; }

        public byte[] Source { get; set; }

        public string ContentType { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime? DateProcessed { get; set; }

        public UserEntity User { get; set; }

        public IEnumerable<TranscribeItemEntity> TranscribeItems { get; set; }
    }
}
