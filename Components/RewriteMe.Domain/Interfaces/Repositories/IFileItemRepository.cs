using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RewriteMe.Domain.Enums;
using RewriteMe.Domain.Transcription;

namespace RewriteMe.Domain.Interfaces.Repositories
{
    public interface IFileItemRepository
    {
        Task<IEnumerable<FileItem>> GetAllAsync(Guid userId, DateTime updatedAfter);

        Task<FileItem> GetAsync(Guid userId, Guid fileItemId);

        Task<DateTime> GetLastUpdateAsync(Guid userId);

        Task AddAsync(FileItem fileItem);

        Task RemoveAsync(Guid userId, Guid fileItemId);

        Task UpdateAsync(FileItem fileItem);

        Task UpdateRecognitionStateAsync(Guid fileItemId, RecognitionState recognitionState);

        Task UpdateDateProcessedAsync(Guid fileItemId);
    }
}
