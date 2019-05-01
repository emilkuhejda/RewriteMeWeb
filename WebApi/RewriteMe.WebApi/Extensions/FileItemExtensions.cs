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
                RecognitionState = fileItem.RecognitionState.ToString(),
                DateCreated = fileItem.DateCreated,
                DateProcessed = fileItem.DateProcessed,
                DateUpdated = fileItem.DateUpdated,
                AudioSourceVersion = fileItem.AudioSourceVersion
            };
        }
    }
}