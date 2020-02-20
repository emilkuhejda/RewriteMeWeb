using RewriteMe.Domain.Dtos;
using RewriteMe.Domain.Settings;

namespace RewriteMe.WebApi.Commands
{
    public class CreateSpeechConfigurationCommand : CommandBase<SpeechConfigurationDto>
    {
        public AppSettings AppSettings { get; set; }
    }
}
