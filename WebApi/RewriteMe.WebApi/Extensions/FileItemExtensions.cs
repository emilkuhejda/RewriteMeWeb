using RewriteMe.Domain.Transcription;
using RewriteMe.WebApi.Dtos;

namespace RewriteMe.WebApi.Extensions
{
    public static class FileItemExtensions
    {
        public static FileItemDto ToDto(this FileItem fileItem)
        {
            return new FileItemDto
            {
                Id = fileItem.Id,
                Name = fileItem.Name,
                FileName = fileItem.FileName,
                Language = fileItem.Language,
                RecognitionStateString = fileItem.RecognitionState.ToString(),
                UploadStatus = fileItem.UploadStatus,
                TotalTimeTicks = fileItem.TotalTime.Ticks,
                TranscribedTimeTicks = fileItem.TranscribedTime.Ticks,
                DateCreated = fileItem.DateCreated,
                DateProcessedUtc = fileItem.DateProcessedUtc,
                DateUpdatedUtc = fileItem.DateUpdatedUtc,
                IsDeleted = fileItem.IsDeleted
            };
        }
    }
}
