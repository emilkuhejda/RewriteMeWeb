using System;
using RewriteMe.Domain.Enums;

namespace RewriteMe.WebApi.Models
{
    public class UpdateRecognitionStateModel
    {
        public Guid FileItemId { get; set; }

        public string FileName { get; set; }

        public RecognitionState RecognitionState { get; set; }

        public Guid ApplicationId { get; set; }
    }
}
