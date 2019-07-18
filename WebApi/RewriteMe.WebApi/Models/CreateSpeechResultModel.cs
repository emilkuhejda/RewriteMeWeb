using System;

namespace RewriteMe.WebApi.Models
{
    public class CreateSpeechResultModel
    {
        public Guid RecognizedAudioSampleId { get; set; }

        public string DisplayText { get; set; }
    }
}
