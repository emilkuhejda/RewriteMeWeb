using RewriteMe.Domain.Transcription;
using RewriteMe.WebApi.Dtos;

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
                Source = transcribeItem.Source,
                StartTimeString = transcribeItem.StartTime.ToString(),
                EndTimeString = transcribeItem.EndTime.ToString(),
                TotalTimeString = transcribeItem.TotalTime.ToString(),
                DateCreated = transcribeItem.DateCreated,
                DateUpdated = transcribeItem.DateUpdated
            };
        }
    }
}
