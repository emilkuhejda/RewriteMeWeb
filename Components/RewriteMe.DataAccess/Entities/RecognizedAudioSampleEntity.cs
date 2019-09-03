using System;
using System.Collections.Generic;

namespace RewriteMe.DataAccess.Entities
{
    public class RecognizedAudioSampleEntity
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public DateTime DateCreated { get; set; }

        public virtual UserEntity User { get; set; }

        public virtual IList<SpeechResultEntity> SpeechResults { get; set; }
    }
}
