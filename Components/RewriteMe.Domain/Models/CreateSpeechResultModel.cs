using System;
using System.ComponentModel.DataAnnotations;

namespace RewriteMe.Domain.Models
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
