﻿using RewriteMe.Domain.Transcription;
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
                TotalTimeTicks = fileItem.TotalTime.Ticks,
                TranscribedTimeTicks = fileItem.TranscribedTime.Ticks,
                DateCreatedUtc = fileItem.DateCreated,
                DateProcessedUtc = fileItem.DateProcessed,
                DateUpdatedUtc = fileItem.DateUpdated,
                IsDeleted = fileItem.IsDeleted
            };
        }
    }
}
