﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RewriteMe.Domain.Enums;
using RewriteMe.Domain.Transcription;

namespace RewriteMe.Domain.Interfaces.Services
{
    public interface IFileItemService
    {
        Task<IEnumerable<FileItem>> GetAllAsync(Guid userId);

        Task<FileItem> GetFileItemWithoutSourceAsync(Guid userId, Guid fileItemId);

        Task<FileItem> GetFileItemAsync(Guid userId, Guid fileItemId);

        Task AddAsync(FileItem fileItem);

        Task RemoveAsync(Guid userId, Guid fileItemId);

        Task UpdateAsync(FileItem fileItem);

        Task UpdateRecognitionStateAsync(Guid fileItemId, RecognitionState recognitionState);

        Task UpdateDateProcessedAsync(Guid fileItemId);
    }
}
