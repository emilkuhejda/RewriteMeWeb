using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Options;
using RewriteMe.Common.Helpers;
using RewriteMe.Domain.Enums;
using RewriteMe.Domain.Interfaces.Repositories;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.Domain.Settings;
using RewriteMe.Domain.Transcription;

namespace RewriteMe.Business.Services
{
    public class StorageService : IStorageService
    {
        private const string ContainerName = "voicipher-storage";
        private const string SourceDirectory = "source";
        private const string TranscriptionsDirectory = "transcriptions";

        private readonly IFileAccessService _fileAccessService;
        private readonly IFileItemRepository _fileItemRepository;
        private readonly ITranscribeItemRepository _transcribeItemRepository;
        private readonly AppSettings _appSettings;

        private BlobContainerClient _containerClient;

        public StorageService(
            IFileAccessService fileAccessService,
            IFileItemRepository fileItemRepository,
            ITranscribeItemRepository transcribeItemRepository,
            IOptions<AppSettings> options)
        {
            _fileAccessService = fileAccessService;
            _fileItemRepository = fileItemRepository;
            _transcribeItemRepository = transcribeItemRepository;
            _appSettings = options.Value;
        }

        private BlobContainerClient ContainerClient => _containerClient ?? (_containerClient = GetContainerClient().GetAwaiter().GetResult());

        public void Migrate()
        {
            AsyncHelper.RunSync(MigrateAsync);
        }

        private async Task MigrateAsync()
        {
            var fileItemsToMigrate = (await _fileItemRepository.GetFileItemsForMigrationAsync().ConfigureAwait(false)).ToList();
            if (!fileItemsToMigrate.Any())
                return;

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

            await UploadFilesAsync(sourceDirectoryPath, GetSourceDirectoryPath(fileItem)).ConfigureAwait(false);
            await UploadFilesAsync(transcriptionsDirectoryPath, GetTranscriptionsDirectoryPath(fileItem)).ConfigureAwait(false);

            ClearFileItemData(fileItem);

            await _fileItemRepository.UpdateStorageAsync(fileItem.Id, StorageSetting.Azure).ConfigureAwait(false);
            await _transcribeItemRepository.UpdateStorageAsync(fileItem.Id, StorageSetting.Azure).ConfigureAwait(false);
        }

        public async Task<byte[]> GetFileItemBytesAsync(FileItem fileItem)
        {
            var path = GetSourceFilePath(fileItem);
            var client = ContainerClient.GetBlobClient(path);

            using (var memoryStream = new MemoryStream())
            {
                await client.DownloadToAsync(memoryStream).ConfigureAwait(false);

                return memoryStream.ToArray();
            }
        }

        public async Task<byte[]> GetTranscribeItemBytesAsync(TranscribeItem transcribeItem, Guid userId)
        {
            var path = GetTranscriptionFilePath(transcribeItem, userId);
            var client = ContainerClient.GetBlobClient(path);

            using (var memoryStream = new MemoryStream())
            {
                await client.DownloadToAsync(memoryStream).ConfigureAwait(false);

                return memoryStream.ToArray();
            }
        }

        private async Task UploadFilesAsync(string directoryPath, string destinationPath)
        {
            var directoryInfo = new DirectoryInfo(directoryPath);
            if (!directoryInfo.Exists)
                return;

            var files = directoryInfo.GetFiles();
            foreach (var fileInfo in files)
            {
                await UpdateFileAsync(fileInfo, destinationPath).ConfigureAwait(false);
            }
        }

        private async Task UpdateFileAsync(FileInfo fileInfo, string destinationPath)
        {
            var filePath = Path.Combine(destinationPath, fileInfo.Name);
            var client = ContainerClient.GetBlobClient(filePath);
            using (var fileStream = File.OpenRead(fileInfo.FullName))
            {
                await client.UploadAsync(fileStream, true).ConfigureAwait(false);
            }
        }

        private async Task<BlobContainerClient> GetContainerClient()
        {
            var blobServiceClient = new BlobServiceClient(_appSettings.AzureStorageAccount.ConnectionString);
            var container = blobServiceClient.GetBlobContainerClient(ContainerName);
            await container.CreateIfNotExistsAsync().ConfigureAwait(false);

            return container;
        }

        private string GetSourceDirectoryPath(FileItem fileItem)
        {
            return Path.Combine(fileItem.UserId.ToString(), fileItem.Id.ToString(), SourceDirectory);
        }

        private string GetSourceFilePath(FileItem fileItem)
        {
            return Path.Combine(fileItem.UserId.ToString(), fileItem.Id.ToString(), SourceDirectory, fileItem.OriginalSourceFileName);
        }

        private string GetTranscriptionFilePath(TranscribeItem transcribeItem, Guid userId)
        {
            return Path.Combine(userId.ToString(), transcribeItem.FileItemId.ToString(), TranscriptionsDirectory, transcribeItem.SourceFileName);
        }

        private string GetTranscriptionsDirectoryPath(FileItem fileItem)
        {
            return Path.Combine(fileItem.UserId.ToString(), fileItem.Id.ToString(), TranscriptionsDirectory);
        }

        private void ClearFileItemData(FileItem fileItem)
        {
            var directoryPath = _fileAccessService.GetFileItemRootDirectory(fileItem.UserId, fileItem.Id);
            Directory.Delete(directoryPath, true);
        }
    }
}
