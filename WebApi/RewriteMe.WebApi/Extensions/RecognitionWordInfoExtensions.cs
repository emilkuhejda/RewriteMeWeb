using RewriteMe.Domain.Dtos;
using RewriteMe.Domain.Transcription;

namespace RewriteMe.WebApi.Extensions
{
    public static class RecognitionWordInfoExtensions
    {
        public static RecognitionWordInfoDto ToDto(this RecognitionWordInfo recognitionWordInfo)
        {
            return new RecognitionWordInfoDto
            {
                Word = recognitionWordInfo.Word,
                StartTimeTicks = recognitionWordInfo.StartTime.Ticks,
                EndTimeTicks = recognitionWordInfo.EndTime.Ticks,
                SpeakerTag = recognitionWordInfo.SpeakerTag
            };
        }
    }
}
