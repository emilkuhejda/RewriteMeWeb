using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using NAudio.Wave;
using RewriteMe.Domain.Enums;
using RewriteMe.Domain.Interfaces.Repositories;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.Domain.Settings;
using RewriteMe.Domain.Transcription;

namespace RewriteMe.Business.Services
{
    public class FileItemService : IFileItemService
    {
        private readonly IFileItemRepository _fileItemRepository;
        private readonly IFileAccessService _fileAccessService;

        public FileItemService(
            IFileItemRepository fileItemRepository,
            IFileAccessService fileAccessService)
        {
            _fileItemRepository = fileItemRepository;
            _fileAccessService = fileAccessService;
        }

        public async Task<bool> ExistsAsync(Guid userId, Guid fileItemId)
        {
            return await _fileItemRepository.ExistsAsync(userId, fileItemId).ConfigureAwait(false);
        }

        public async Task<bool> ExistsAsync(Guid fileItemId, string fileName)
        {
            return await _fileItemRepository.ExistsAsync(fileItemId, fileName).ConfigureAwait(false);
        }

        public async Task<IEnumerable<FileItem>> GetAllAsync(Guid userId, DateTime updatedAfter, Guid applicationId)
        {
            return await _fileItemRepository.GetAllAsync(userId, updatedAfter, applicationId).ConfigureAwait(false);
        }

        public async Task<IEnumerable<FileItem>> GetAllForUserAsync(Guid userId)
        {
            return await _fileItemRepository.GetAllForUserAsync(userId).ConfigureAwait(false);
        }

        public async Task<IEnumerable<FileItem>> GetTemporaryDeletedFileItemsAsync(Guid userId)
        {
            return await _fileItemRepository.GetTemporaryDeletedFileItemsAsync(userId).ConfigureAwait(false);
        }

        public async Task<IEnumerable<Guid>> GetAllDeletedIdsAsync(Guid userId, DateTime updatedAfter, Guid applicationId)
        {
            return await _fileItemRepository.GetAllDeletedIdsAsync(userId, updatedAfter, applicationId).ConfigureAwait(false);
        }

        public async Task<FileItem> GetAsync(Guid userId, Guid fileItemId)
        {
            return await _fileItemRepository.GetAsync(userId, fileItemId).ConfigureAwait(false);
        }

        public async Task<FileItem> GetAsAdminAsync(Guid fileItemId)
        {
            return await _fileItemRepository.GetAsAdminAsync(fileItemId).ConfigureAwait(false);
        }

        public async Task<TimeSpan> GetDeletedFileItemsTotalTime(Guid userId)
        {
            return await _fileItemRepository.GetDeletedFileItemsTotalTime(userId).ConfigureAwait(false);
        }

        public async Task<DateTime> GetLastUpdateAsync(Guid userId)
        {
            return await _fileItemRepository.GetLastUpdateAsync(userId).ConfigureAwait(false);
        }

        public async Task<DateTime> GetDeletedLastUpdateAsync(Guid userId)
        {
            return await _fileItemRepository.GetDeletedLastUpdateAsync(userId).ConfigureAwait(false);
        }

        public async Task AddAsync(FileItem fileItem)
        {
            await _fileItemRepository.AddAsync(fileItem).ConfigureAwait(false);
        }

        public async Task DeleteAsync(Guid userId, Guid fileItemId, Guid applicationId)
        {
            await _fileItemRepository.DeleteAsync(userId, fileItemId, applicationId).ConfigureAwait(false);
        }

        public async Task DeleteAllAsync(Guid userId, IEnumerable<DeletedFileItem> fileItems, Guid applicationId)
        {
            await _fileItemRepository.DeleteAllAsync(userId, fileItems, applicationId).ConfigureAwait(false);
        }

        public async Task PermanentDeleteAllAsync(Guid userId, IEnumerable<Guid> fileItemIds, Guid applicationId)
        {
            await _fileItemRepository.PermanentDeleteAllAsync(userId, fileItemIds, applicationId).ConfigureAwait(false);
        }

        public async Task RestoreAllAsync(Guid userId, IEnumerable<Guid> fileItemIds, Guid applicationId)
        {
            await _fileItemRepository.RestoreAllAsync(userId, fileItemIds, applicationId).ConfigureAwait(false);
        }

        public async Task UpdateLanguageAsync(Guid fileItemId, string language, Guid applicationId)
        {
            await _fileItemRepository.UpdateLanguageAsync(fileItemId, language, applicationId).ConfigureAwait(false);
        }

        public async Task UpdateAsync(FileItem fileItem)
        {
            await _fileItemRepository.UpdateAsync(fileItem).ConfigureAwait(false);
        }

        public async Task UpdateSourceFileNameAsync(Guid fileItemId, string sourceFileName)
        {
            await _fileItemRepository.UpdateSourceFileNameAsync(fileItemId, sourceFileName).ConfigureAwait(false);
        }

        public async Task UpdateRecognitionStateAsync(Guid fileItemId, RecognitionState recognitionState, Guid applicationId)
        {
            await _fileItemRepository.UpdateRecognitionStateAsync(fileItemId, recognitionState, applicationId).ConfigureAwait(false);
        }

        public async Task UpdateDateProcessedAsync(Guid fileItemId, Guid applicationId)
        {
            await _fileItemRepository.UpdateDateProcessedAsync(fileItemId, applicationId).ConfigureAwait(false);
        }

        public async Task UpdateTranscribedTimeAsync(Guid fileItemId, TimeSpan transcribedTime)
        {
            await _fileItemRepository.UpdateTranscribedTimeAsync(fileItemId, transcribedTime).ConfigureAwait(false);
        }

        public async Task RemoveSourceFileNameAsync(FileItem fileItem)
        {
            await _fileItemRepository.UpdateSourceFileNameAsync(fileItem.Id, null).ConfigureAwait(false);

            var filePath = _fileAccessService.GetFileItemPath(fileItem);
            File.Delete(filePath);
        }

        public async Task<byte[]> GetAudioSource(Guid fileItemId)
        {
            var fileItem = await _fileItemRepository.GetAsync(fileItemId).ConfigureAwait(false);
            var fileItemPath = _fileAccessService.GetFileItemPath(fileItem);
            if (!File.Exists(fileItemPath))
                return Array.Empty<byte>();

            return await File.ReadAllBytesAsync(fileItemPath).ConfigureAwait(false);
        }

        public async Task<UploadedFile> UploadFileAsync(Guid fileItemId, byte[] uploadedFileSource)
        {
            var directoryPath = _fileAccessService.GetRootPath();
            var uploadDirectoryPath = Path.Combine(directoryPath, fileItemId.ToString());
            Directory.CreateDirectory(uploadDirectoryPath);

            var uploadedFileName = Guid.NewGuid().ToString();
            var uploadedFilePath = Path.Combine(uploadDirectoryPath, uploadedFileName);

            await File.WriteAllBytesAsync(uploadedFilePath, uploadedFileSource).ConfigureAwait(false);

            return new UploadedFile
            {
                FileName = uploadedFileName,
                FilePath = uploadedFilePath,
                DirectoryPath = uploadDirectoryPath
            };
        }

        public TimeSpan GetAudioTotalTime(string filePath)
        {
            using (var reader = new MediaFoundationReader(filePath))
            {
                return reader.TotalTime;
            }
        }
    }
}
