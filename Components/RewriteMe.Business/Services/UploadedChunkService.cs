using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using RewriteMe.Domain.Enums;
using RewriteMe.Domain.Interfaces.Repositories;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.Domain.Transcription;

namespace RewriteMe.Business.Services
{
    public class UploadedChunkService : IUploadedChunkService
    {
        private readonly IFileAccessService _fileAccessService;
        private readonly IUploadedChunkRepository _uploadedChunkRepository;

        public UploadedChunkService(
            IFileAccessService fileAccessService,
            IUploadedChunkRepository uploadedChunkRepository)
        {
            _fileAccessService = fileAccessService;
            _uploadedChunkRepository = uploadedChunkRepository;
        }

        public async Task SaveAsync(Guid fileItemId, int order, StorageSetting storageSetting, Guid applicationId, byte[] source, CancellationToken cancellationToken)
        {
            var uploadedChunk = new UploadedChunk
            {
                Id = Guid.NewGuid(),
                FileItemId = fileItemId,
                ApplicationId = applicationId,
                Source = storageSetting == StorageSetting.Database ? source : Array.Empty<byte>(),
                Order = order,
                DateCreatedUtc = DateTime.UtcNow
            };

            if (storageSetting == StorageSetting.Database)
            {
                var directoryPath = _fileAccessService.GetChunksFileItemStoragePath(fileItemId);
                var filePath = Path.Combine(directoryPath, uploadedChunk.Id.ToString());
                await File.WriteAllBytesAsync(filePath, source, cancellationToken).ConfigureAwait(false);
            }

            cancellationToken.ThrowIfCancellationRequested();
            await AddAsync(uploadedChunk).ConfigureAwait(false);
        }

        private async Task AddAsync(UploadedChunk uploadedChunk)
        {
            await _uploadedChunkRepository.AddAsync(uploadedChunk).ConfigureAwait(false);
        }

        public async Task<IEnumerable<UploadedChunk>> GetAllAsync(Guid fileItemId, Guid applicationId, CancellationToken cancellationToken)
        {
            return await _uploadedChunkRepository.GetAllAsync(fileItemId, applicationId, cancellationToken).ConfigureAwait(false);
        }

        public async Task DeleteAsync(Guid fileItemId, Guid applicationId)
        {
            await _uploadedChunkRepository.DeleteAsync(fileItemId, applicationId).ConfigureAwait(false);
        }

        public async Task CleanOutdatedChunksAsync()
        {
            await _uploadedChunkRepository.CleanOutdatedChunksAsync().ConfigureAwait(false);
        }
    }
}
