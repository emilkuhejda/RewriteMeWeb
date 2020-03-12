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

        Task<bool> ExistsAsync(Guid fileItemId, string fileName);

        Task<IEnumerable<FileItem>> GetAllAsync(Guid userId, DateTime updatedAfter, Guid applicationId);

        Task<IEnumerable<FileItem>> GetAllForUserAsync(Guid userId);

        Task<IEnumerable<FileItem>> GetTemporaryDeletedFileItemsAsync(Guid userId);

        Task<IEnumerable<Guid>> GetAllDeletedIdsAsync(Guid userId, DateTime updatedAfter, Guid applicationId);

        Task<FileItem> GetAsync(Guid fileItemId);

        Task<FileItem> GetAsync(Guid userId, Guid fileItemId);

        Task<FileItem> GetAsAdminAsync(Guid fileItemId);

        Task<TimeSpan> GetDeletedFileItemsTotalTimeAsync(Guid userId);

        Task<DateTime> GetLastUpdateAsync(Guid userId);

        Task<DateTime> GetDeletedLastUpdateAsync(Guid userId);

        Task<bool> IsInPreparedStateAsync(Guid fileItemId);

        Task AddAsync(FileItem fileItem);

        Task DeleteAsync(Guid userId, Guid fileItemId, Guid applicationId);

        Task DeleteAllAsync(Guid userId, IEnumerable<DeletedFileItem> fileItems, Guid applicationId);

        Task PermanentDeleteAllAsync(Guid userId, IEnumerable<Guid> fileItemIds, Guid applicationId);

        Task RestoreAllAsync(Guid userId, IEnumerable<Guid> fileItemIds, Guid applicationId);

        Task UpdateLanguageAsync(Guid fileItemId, string language, Guid applicationId);

        Task UpdateAsync(FileItem fileItem);

        Task UpdateSourceFileNameAsync(Guid fileItemId, string sourceFileName);

        Task UpdateRecognitionStateAsync(Guid fileItemId, RecognitionState recognitionState, Guid applicationId);

        Task UpdateDateProcessedAsync(Guid fileItemId, Guid applicationId);

        Task UpdateTranscribedTimeAsync(Guid fileItemId, TimeSpan transcribedTime);

        Task UpdateUploadStatus(Guid fileItemId, UploadStatus uploadStatus, Guid applicationId);

        Task UpdateStorageAsync(Guid fileItemId, StorageSetting storageSetting);

        Task MarkAsCleanedAsync(Guid fileItemId);

        Task<TimeSpan> GetTranscribedTotalSecondsAsync(Guid userId);

        Task<IEnumerable<(Guid FileItemId, Guid UserId)>> GetFileItemsForCleaningAsync(DateTime deleteBefore, bool forceCleanUp);

        Task<IEnumerable<FileItem>> GetFileItemsForMigrationAsync();

        Task CleanSourceDataAsync(Guid fileItemId);
    }
}
