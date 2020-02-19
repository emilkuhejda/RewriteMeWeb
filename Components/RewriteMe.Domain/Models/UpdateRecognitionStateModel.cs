using System;
using System.ComponentModel.DataAnnotations;
using RewriteMe.Domain.Enums;

namespace RewriteMe.Domain.Models
{
    public class UpdateRecognitionStateModel
    {
        [Required]
        public Guid FileItemId { get; set; }

        [Required]
        public string FileName { get; set; }

        [Required]
        public RecognitionState RecognitionState { get; set; }

        [Required]
        public Guid ApplicationId { get; set; }
    }
}
