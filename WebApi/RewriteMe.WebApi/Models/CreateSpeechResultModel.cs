using System;
using System.ComponentModel.DataAnnotations;

namespace RewriteMe.WebApi.Models
{
    public class CreateSpeechResultModel
    {
        [Required]
        public Guid SpeechResultId { get; set; }

        [Required]
        public Guid RecognizedAudioSampleId { get; set; }

        public string DisplayText { get; set; }
    }
}
