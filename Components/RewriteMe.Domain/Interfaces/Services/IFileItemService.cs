using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RewriteMe.Domain.Enums;
using RewriteMe.Domain.Settings;
using RewriteMe.Domain.Transcription;

namespace RewriteMe.Domain.Interfaces.Services
{
    public interface IFileItemService
    {
        Task<bool> ExistsAsync(Guid userId, Guid fileItemId);

        Task<bool> ExistsAsync(Guid fileItemId, string fileName);

        Task<IEnumerable<FileItem>> GetAllAsync(Guid userId, DateTime updatedAfter, Guid applicationId);

        Task<IEnumerable<FileItem>> GetAllForUserAsync(Guid userId);

        Task<IEnumerable<FileItem>> GetTemporaryDeletedFileItemsAsync(Guid userId);

        Task<IEnumerable<Guid>> GetAllDeletedIdsAsync(Guid userId, DateTime updatedAfter, Guid applicationId);

        Task<FileItem> GetAsync(Guid userId, Guid fileItemId);

        Task<FileItem> GetAsAdminAsync(Guid fileItemId);

        Task<DateTime> GetLastUpdateAsync(Guid userId);

        Task<TimeSpan> GetDeletedFileItemsTotalTimeAsync(Guid userId);

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

        Task RemoveSourceFileAsync(FileItem fileItem);

        Task<byte[]> GetAudioSourceAsync(FileItem fileItem);

        Task<string> GetOriginalFileItemPathAsync(FileItem fileItem, string directoryPath);

        Task<bool> ConvertedFileItemSourceExistsAsync(FileItem fileItem);

        string CreateUploadDirectoryIfNeeded(Guid fileItemId, bool isTemporaryStorage);

        Task<UploadedFile> UploadFileToStorageAsync(Guid fileItemId, byte[] uploadedFileSource);

        void CleanUploadedData(string directoryPath);

        TimeSpan? GetAudioTotalTime(string filePath);
    }
}
