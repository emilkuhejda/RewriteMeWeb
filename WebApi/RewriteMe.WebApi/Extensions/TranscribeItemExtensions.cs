using RewriteMe.Domain.Dtos;
using RewriteMe.Domain.Transcription;

namespace RewriteMe.WebApi.Extensions
{
    public static class TranscribeItemExtensions
    {
        public static TranscribeItemDto ToDto(this TranscribeItem transcribeItem)
        {
            return new TranscribeItemDto
            {
                Id = transcribeItem.Id,
                FileItemId = transcribeItem.FileItemId,
                Alternatives = transcribeItem.Alternatives,
                UserTranscript = transcribeItem.UserTranscript,
                StartTimeTicks = transcribeItem.StartTime.Ticks,
                EndTimeTicks = transcribeItem.EndTime.Ticks,
                TotalTimeTicks = transcribeItem.TotalTime.Ticks,
                DateCreatedUtc = transcribeItem.DateCreatedUtc,
                DateUpdatedUtc = transcribeItem.DateUpdatedUtc
            };
        }
    }
}
