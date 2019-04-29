using System;
using System.Collections.Generic;
using RewriteMe.Domain.Enums;

namespace RewriteMe.DataAccess.Entities
{
    public class FileItemEntity
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public string Name { get; set; }

        public string FileName { get; set; }

        public string Language { get; set; }

        public RecognitionState RecognitionState { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime? DateProcessed { get; set; }

        public DateTime DateUpdated { get; set; }

        public int AudioSourceVersion { get; set; }

        public virtual UserEntity User { get; set; }

        public virtual AudioSourceEntity AudioSource { get; set; }

        public virtual IList<TranscribeItemEntity> TranscribeItems { get; set; }
    }
}
