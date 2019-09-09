using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RewriteMe.Domain.Enums;
using RewriteMe.Domain.Settings;
using RewriteMe.Domain.Transcription;

namespace RewriteMe.Domain.Interfaces.Repositories
{
    public interface IFileItemRepository
    {
        Task<bool> ExistsAsync(Guid userId, Guid fileItemId);

        Task<IEnumerable<FileItem>> GetAllAsync(Guid userId, DateTime updatedAfter, Guid applicationId);

        Task<IEnumerable<FileItem>> GetTemporaryDeletedFileItemsAsync(Guid userId);

        Task<IEnumerable<Guid>> GetAllDeletedIdsAsync(Guid userId, DateTime updatedAfter, Guid applicationId);

        Task<FileItem> GetAsync(Guid fileItemId);

        Task<FileItem> GetAsync(Guid userId, Guid fileItemId);

        Task<TimeSpan> GetDeletedFileItemsTotalTime(Guid userId);

        Task<DateTime> GetLastUpdateAsync(Guid userId);

        Task<DateTime> GetDeletedLastUpdateAsync(Guid userId);

        Task AddAsync(FileItem fileItem);

        Task DeleteAsync(Guid userId, Guid fileItemId, Guid applicationId);

        Task DeleteAllAsync(Guid userId, IEnumerable<DeletedFileItem> fileItems, Guid applicationId, bool isPermanentDelete);

        Task RestoreAllAsync(Guid userId, IEnumerable<Guid> fileItemIds, Guid applicationId);

        Task UpdateLanguageAsync(Guid fileItemId, string language, Guid applicationId);

        Task UpdateAsync(FileItem fileItem);

        Task UpdateSourceFileNameAsync(Guid fileItemId, string sourceFileName);

        Task UpdateRecognitionStateAsync(Guid fileItemId, RecognitionState recognitionState, Guid applicationId);

        Task UpdateDateProcessedAsync(Guid fileItemId, Guid applicationId);

        Task UpdateTranscribedTimeAsync(Guid fileItemId, TimeSpan transcribedTime);

        Task<TimeSpan> GetTranscribedTotalSeconds(Guid userId);
    }
}
