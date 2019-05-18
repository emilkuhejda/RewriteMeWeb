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
                TotalTimeString = fileItem.TotalTime.ToString(),
                DateCreated = fileItem.DateCreated,
                DateProcessed = fileItem.DateProcessed,
                DateUpdated = fileItem.DateUpdated,
                AudioSourceVersion = fileItem.AudioSourceVersion,
                IsDeleted = fileItem.IsDeleted,
            };
        }
    }
}
