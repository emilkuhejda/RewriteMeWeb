using System;
using System.Collections.Generic;

namespace RewriteMe.DataAccess.Entities
{
    public class RecognizedAudioSampleEntity
    {
        public Guid Id { get; set; }

        public DateTime DateCreated { get; set; }

        public virtual IEnumerable<SpeechResultEntity> SpeechResults { get; set; }
    }
}
