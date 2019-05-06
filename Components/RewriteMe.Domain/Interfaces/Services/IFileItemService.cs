﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RewriteMe.Domain.Enums;
using RewriteMe.Domain.Transcription;

namespace RewriteMe.Domain.Interfaces.Services
{
    public interface IFileItemService
    {
        Task<bool> ExistsAsync(Guid userId, Guid fileItemId);

        Task<IEnumerable<FileItem>> GetAllAsync(Guid userId, DateTime updatedAfter, Guid applicationId);

        Task<FileItem> GetAsync(Guid userId, Guid fileItemId);

        Task<DateTime> GetLastUpdateAsync(Guid userId);

        Task AddAsync(FileItem fileItem);

        Task RemoveAsync(Guid userId, Guid fileItemId);

        Task UpdateLanguageAsync(Guid fileItemId, string language, Guid applicationId);

        Task UpdateAsync(FileItem fileItem);

        Task UpdateRecognitionStateAsync(Guid fileItemId, RecognitionState recognitionState, Guid applicationId);

        Task UpdateDateProcessedAsync(Guid fileItemId, Guid applicationId);
    }
}
