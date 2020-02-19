using System.Collections.Generic;
using RewriteMe.Domain.Dtos;
using RewriteMe.Domain.Recording;

namespace RewriteMe.WebApi.Commands
{
    public class UpdateSpeechResultsCommand : CommandBase<TimeSpanWrapperDto>
    {
        public IList<SpeechResult> SpeechResults { get; set; }
    }
}
