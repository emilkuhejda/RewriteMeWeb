﻿using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Options;
using RewriteMe.Domain.Enums;
using RewriteMe.Domain.Interfaces.Repositories;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.Domain.Settings;
using RewriteMe.Domain.Transcription;
using Serilog;

namespace RewriteMe.Business.Services
{
    public class StorageService : IStorageService
    {
        private const string SourceDirectory = "source";
        private const string TranscriptionsDirectory = "transcriptions";

        private readonly IFileAccessService _fileAccessService;
        private readonly IFileItemRepository _fileItemRepository;
        private readonly ITranscribeItemRepository _transcribeItemRepository;
        private readonly AppSettings _appSettings;
        private readonly ILogger _logger;

        public StorageService(
            IFileAccessService fileAccessService,
            IFileItemRepository fileItemRepository,
            ITranscribeItemRepository transcribeItemRepository,
            IOptions<AppSettings> options,
            ILogger logger)
        {
            _fileAccessService = fileAccessService;
            _fileItemRepository = fileItemRepository;
            _transcribeItemRepository = transcribeItemRepository;
            _appSettings = options.Value;
            _logger = logger.ForContext<StorageService>();
        }

        public async Task MigrateAsync()
        {
            var fileItemsToMigrate = (await _fileItemRepository.GetFileItemsForMigrationAsync().ConfigureAwait(false)).ToList();
            if (!fileItemsToMigrate.Any())
                return;

            _logger.Information("Start migration to Azure.");

            foreach (var fileItem in fileItemsToMigrate)
            {
                await MigrateFileItemAsync(fileItem).ConfigureAwait(false);
            }
        }

        private async Task MigrateFileItemAsync(FileItem fileItem)
        {
            if (!fileItem.CanMigrate)
                return;

            var sourceDirectoryPath = _fileAccessService.GetFileItemSourceDirectory(fileItem.UserId, fileItem.Id);
            var transcriptionsDirectoryPath = _fileAccessService.GetTranscriptionsDirectoryPath(fileItem.UserId, fileItem.Id);

            await UploadFilesAsync(sourceDirectoryPath, GetSourceDirectoryPath(fileItem), fileItem.UserId).ConfigureAwait(false);
            await UploadFilesAsync(transcriptionsDirectoryPath, GetTranscriptionsDirectoryPath(fileItem), fileItem.UserId).ConfigureAwait(false);

            ClearFileItemData(fileItem);

            await _fileItemRepository.UpdateStorageAsync(fileItem.Id, StorageSetting.Azure).ConfigureAwait(false);
            await _transcribeItemRepository.UpdateStorageAsync(fileItem.Id, StorageSetting.Azure).ConfigureAwait(false);

            _logger.Information($"File item '{fileItem.Id}' was migrated.");
        }

        public async Task<byte[]> GetFileItemBytesAsync(FileItem fileItem)
        {
            var path = GetSourceFilePath(fileItem);
            var container = await GetContainerClient(fileItem.UserId).ConfigureAwait(false);
            var client = container.GetBlobClient(path);

            using (var memoryStream = new MemoryStream())
            {
                await client.DownloadToAsync(memoryStream).ConfigureAwait(false);

                return memoryStream.ToArray();
            }
        }

        public async Task<byte[]> GetTranscribeItemBytesAsync(TranscribeItem transcribeItem, Guid userId)
        {
            var path = GetTranscriptionFilePath(transcribeItem);
            var container = await GetContainerClient(userId).ConfigureAwait(false);
            var client = container.GetBlobClient(path);

            using (var memoryStream = new MemoryStream())
            {
                await client.DownloadToAsync(memoryStream).ConfigureAwait(false);

                return memoryStream.ToArray();
            }
        }

        public async Task DeleteFileItemFolderDataAsync(Guid userId, Guid fileItemId)
        {
            var container = await GetContainerClient(userId).ConfigureAwait(false);
            var blobItems = container.GetBlobs().AsPages().SelectMany(x => x.Values).ToList();

            foreach (var blobItem in blobItems)
            {
                if (blobItem.Name.Contains(fileItemId.ToString(), StringComparison.InvariantCulture))
                {
                    var client = container.GetBlobClient(blobItem.Name);
                    await client.DeleteIfExistsAsync().ConfigureAwait(false);
                }
            }

            _logger.Information($"File item folder data '{fileItemId}' was deleted.");
        }

        public async Task DeleteFileItemSourceAsync(FileItem fileItem)
        {
            var path = GetSourceFilePath(fileItem);
            var container = await GetContainerClient(fileItem.UserId).ConfigureAwait(false);
            var client = container.GetBlobClient(path);
            await client.DeleteIfExistsAsync().ConfigureAwait(false);

            _logger.Information($"File item source '{path}' was deleted.");
        }

        public async Task DeleteContainerAsync(Guid userId)
        {
            var client = await GetContainerClient(userId).ConfigureAwait(false);
            await client.DeleteIfExistsAsync().ConfigureAwait(false);

            _logger.Information($"Container was deleted. Name = {client.Name}, Account name = {client.AccountName}.");
        }

        private async Task UploadFilesAsync(string directoryPath, string destinationPath, Guid userId)
        {
            var directoryInfo = new DirectoryInfo(directoryPath);
            if (!directoryInfo.Exists)
                return;

            var files = directoryInfo.GetFiles();
            foreach (var fileInfo in files)
            {
                await UpdateFileAsync(fileInfo, destinationPath, userId).ConfigureAwait(false);
            }
        }

        private async Task UpdateFileAsync(FileInfo fileInfo, string destinationPath, Guid userId)
        {
            var filePath = Path.Combine(destinationPath, fileInfo.Name);
            var container = await GetContainerClient(userId).ConfigureAwait(false);
            var client = container.GetBlobClient(filePath);
            using (var fileStream = File.OpenRead(fileInfo.FullName))
            {
                await client.UploadAsync(fileStream, true).ConfigureAwait(false);

                _logger.Information($"File '{filePath}' was updated.");
            }
        }

        private async Task<BlobContainerClient> GetContainerClient(Guid userId)
        {
            var blobServiceClient = new BlobServiceClient(_appSettings.AzureStorageAccount.ConnectionString);
            var container = blobServiceClient.GetBlobContainerClient(userId.ToString());
            await container.CreateIfNotExistsAsync().ConfigureAwait(false);

            return container;
        }

        private string GetSourceDirectoryPath(FileItem fileItem)
        {
            return Path.Combine(fileItem.Id.ToString(), SourceDirectory);
        }

        private string GetSourceFilePath(FileItem fileItem)
        {
            return Path.Combine(fileItem.Id.ToString(), SourceDirectory, fileItem.OriginalSourceFileName);
        }

        private string GetTranscriptionFilePath(TranscribeItem transcribeItem)
        {
            return Path.Combine(transcribeItem.FileItemId.ToString(), TranscriptionsDirectory, transcribeItem.SourceFileName);
        }

        private string GetTranscriptionsDirectoryPath(FileItem fileItem)
        {
            return Path.Combine(fileItem.Id.ToString(), TranscriptionsDirectory);
        }

        private void ClearFileItemData(FileItem fileItem)
        {
            var directoryPath = _fileAccessService.GetFileItemRootDirectory(fileItem.UserId, fileItem.Id);
            Directory.Delete(directoryPath, true);

            _logger.Information($"File item data in destination '{directoryPath}' were deleted.");
        }
    }
}
