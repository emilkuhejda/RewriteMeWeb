using System;
using RewriteMe.Domain.Dtos;

namespace RewriteMe.WebApi.Commands
{
    public class CanRunRecognitionCommand : CommandBase<OkDto>
    {
        public Guid FileItemId { get; set; }

        public string Language { get; set; }

        public Guid ApplicationId { get; set; }
    }
}
