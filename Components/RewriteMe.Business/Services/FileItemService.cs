using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NAudio.Wave;
using RewriteMe.Business.Configuration;
using RewriteMe.Common.Utils;
using RewriteMe.Domain.Enums;
using RewriteMe.Domain.Interfaces.Repositories;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.Domain.Settings;
using RewriteMe.Domain.Transcription;
using Serilog;

namespace RewriteMe.Business.Services
{
    public class FileItemService : IFileItemService
    {
        private readonly IFileItemSourceService _fileItemSourceService;
        private readonly IStorageService _storageService;
        private readonly IInternalValueService _internalValueService;
        private readonly IFileAccessService _fileAccessService;
        private readonly IFileItemRepository _fileItemRepository;
        private readonly IFileItemSourceRepository _fileItemSourceRepository;
        private readonly ILogger _logger;

        public FileItemService(
            IFileItemSourceService fileItemSourceService,
            IStorageService storageService,
            IInternalValueService internalValueService,
            IFileAccessService fileAccessService,
            IFileItemRepository fileItemRepository,
            IFileItemSourceRepository fileItemSourceRepository,
            ILogger logger)
        {
            _fileItemSourceService = fileItemSourceService;
            _storageService = storageService;
            _internalValueService = internalValueService;
            _fileAccessService = fileAccessService;
            _fileItemRepository = fileItemRepository;
            _fileItemSourceRepository = fileItemSourceRepository;
            _logger = logger.ForContext<FileItemService>();
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

        public async Task<TimeSpan> GetDeletedFileItemsTotalTimeAsync(Guid userId)
        {
            return await _fileItemRepository.GetDeletedFileItemsTotalTimeAsync(userId).ConfigureAwait(false);
        }

        public async Task<DateTime> GetLastUpdateAsync(Guid userId)
        {
            return await _fileItemRepository.GetLastUpdateAsync(userId).ConfigureAwait(false);
        }

        public async Task<DateTime> GetDeletedLastUpdateAsync(Guid userId)
        {
            return await _fileItemRepository.GetDeletedLastUpdateAsync(userId).ConfigureAwait(false);
        }

        public async Task<bool> IsInPreparedStateAsync(Guid fileItemId)
        {
            return await _fileItemRepository.IsInPreparedStateAsync(fileItemId).ConfigureAwait(false);
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

        public async Task UpdateUploadStatusAsync(Guid fileItemId, UploadStatus uploadStatus, Guid applicationId)
        {
            await _fileItemRepository.UpdateUploadStatusAsync(fileItemId, uploadStatus, applicationId).ConfigureAwait(false);
        }

        public async Task RemoveSourceFileAsync(FileItem fileItem)
        {
            try
            {
                await _fileItemRepository.UpdateSourceFileNameAsync(fileItem.Id, null).ConfigureAwait(false);
                await _fileItemSourceRepository.RemoveAsync(fileItem.Id).ConfigureAwait(false);

                var sourceDirectory = _fileAccessService.GetFileItemSourceDirectory(fileItem.UserId, fileItem.Id);
                if (Directory.Exists(sourceDirectory))
                {
                    Directory.Delete(sourceDirectory, true);
                }

                await _storageService.DeleteFileItemSourceAsync(fileItem).ConfigureAwait(false);

                if (fileItem.Storage == StorageSetting.Azure)
                {
                    await _fileItemRepository.UpdateStorageAsync(fileItem.Id, StorageSetting.Disk).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                _logger.Warning($"File item source for file item ID = '{fileItem.Id}' was not correctly deleted. [{fileItem.UserId}]");
                _logger.Warning(ExceptionFormatter.FormatException(ex));
            }
        }

        public async Task<byte[]> GetAudioSourceAsync(FileItem fileItem)
        {
            if (fileItem.Storage == StorageSetting.Database)
                return GetAudioSourceFromDatabaseAsync(fileItem.Id);

            var fileItemPath = _fileAccessService.GetFileItemPath(fileItem);
            if (!File.Exists(fileItemPath))
            {
                if (fileItem.Storage == StorageSetting.Azure)
                {
                    return await _storageService.GetFileItemBytesAsync(fileItem).ConfigureAwait(false);
                }

                return GetAudioSourceFromDatabaseAsync(fileItem.Id);
            }

            return await File.ReadAllBytesAsync(fileItemPath).ConfigureAwait(false);
        }

        private byte[] GetAudioSourceFromDatabaseAsync(Guid fileItemId)
        {
            var fileItemSource = _fileItemSourceService.GetFileItemSource(fileItemId);
            return fileItemSource?.Source ?? Array.Empty<byte>();
        }

        public async Task<string> GetOriginalFileItemPathAsync(FileItem fileItem, string directoryPath)
        {
            if (fileItem.Storage == StorageSetting.Database)
                return await GetMaterializedFileItemPathAsync(fileItem.Id, directoryPath).ConfigureAwait(false);

            if (fileItem.Storage == StorageSetting.Azure)
                return await GetMaterializedFileItemFromStoragePathAsync(fileItem, directoryPath).ConfigureAwait(false);

            var filePath = _fileAccessService.GetOriginalFileItemPath(fileItem);
            if (!File.Exists(filePath))
                return await GetMaterializedFileItemPathAsync(fileItem.Id, directoryPath).ConfigureAwait(false);

            return filePath;
        }

        private async Task<string> GetMaterializedFileItemPathAsync(Guid fileItemId, string directoryPath)
        {
            var fileItemSource = _fileItemSourceService.GetFileItemSource(fileItemId);
            if (fileItemSource.OriginalSource == null || !fileItemSource.OriginalSource.Any())
                return null;

            var tempFilePath = Path.Combine(directoryPath, $"{Guid.NewGuid()}.wav");
            await File.WriteAllBytesAsync(tempFilePath, fileItemSource.OriginalSource).ConfigureAwait(false);
            return tempFilePath;
        }

        private async Task<string> GetMaterializedFileItemFromStoragePathAsync(FileItem fileItem, string directoryPath)
        {
            var source = await _storageService.GetFileItemBytesAsync(fileItem).ConfigureAwait(false);
            var tempFilePath = Path.Combine(directoryPath, $"{Guid.NewGuid()}.wav");
            await File.WriteAllBytesAsync(tempFilePath, source).ConfigureAwait(false);
            return tempFilePath;
        }

        public async Task<bool> ConvertedFileItemSourceExistsAsync(FileItem fileItem)
        {
            if (fileItem.RecognitionState == RecognitionState.None)
                return false;

            if (fileItem.Storage == StorageSetting.Database)
                return await _fileItemSourceService.HasFileItemSourceAsync(fileItem.Id).ConfigureAwait(false);

            if (!string.IsNullOrWhiteSpace(fileItem.SourceFileName))
            {
                var convertedFilePath = _fileAccessService.GetFileItemPath(fileItem);
                if (File.Exists(convertedFilePath))
                    return true;

                return await _fileItemSourceService.HasFileItemSourceAsync(fileItem.Id).ConfigureAwait(false);
            }

            return false;
        }

        public string CreateUploadDirectoryIfNeeded(Guid userId, Guid fileItemId, bool isTemporaryStorage)
        {
            if (isTemporaryStorage)
            {
                var tempDirectoryPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
                Directory.CreateDirectory(tempDirectoryPath);

                return tempDirectoryPath;
            }

            var uploadDirectoryPath = _fileAccessService.GetFileItemSourceDirectory(userId, fileItemId);
            if (!Directory.Exists(uploadDirectoryPath))
                Directory.CreateDirectory(uploadDirectoryPath);

            return uploadDirectoryPath;
        }

        public async Task<UploadedFile> UploadFileToStorageAsync(Guid userId, Guid fileItemId, byte[] uploadedFileSource)
        {
            var storageSetting = await _internalValueService.GetValueAsync(InternalValues.StorageSetting).ConfigureAwait(false);

            var uploadDirectoryPath = CreateUploadDirectoryIfNeeded(userId, fileItemId, storageSetting == StorageSetting.Database);
            var uploadedFileName = Guid.NewGuid().ToString();
            var uploadedFilePath = Path.Combine(uploadDirectoryPath, uploadedFileName);

            CleanDirectory(uploadDirectoryPath);

            await File.WriteAllBytesAsync(uploadedFilePath, uploadedFileSource).ConfigureAwait(false);

            return new UploadedFile
            {
                FileName = uploadedFileName,
                FilePath = uploadedFilePath,
                DirectoryPath = uploadDirectoryPath
            };
        }

        private void CleanDirectory(string path)
        {
            Directory.Delete(path, true);
            Directory.CreateDirectory(path);
        }

        public void CleanUploadedData(string directoryPath)
        {
            Directory.Delete(directoryPath, true);

            _logger.Information($"File in destination '{directoryPath}' was deleted.");
        }

        public TimeSpan? GetAudioTotalTime(string filePath)
        {
            try
            {
                using (var reader = new MediaFoundationReader(filePath))
                {
                    return reader.TotalTime;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
