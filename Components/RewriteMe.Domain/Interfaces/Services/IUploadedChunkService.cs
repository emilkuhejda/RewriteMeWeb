using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using RewriteMe.Domain.Enums;
using RewriteMe.Domain.Transcription;

namespace RewriteMe.Domain.Interfaces.Services
{
    public interface IUploadedChunkService
    {
        Task SaveAsync(Guid fileItemId, int order, StorageSetting storageSetting, Guid applicationId, byte[] source, CancellationToken cancellationToken);

        Task<IEnumerable<UploadedChunk>> GetAllAsync(Guid fileItemId, Guid applicationId, CancellationToken cancellationToken);

        Task DeleteAsync(Guid fileItemId, Guid applicationId);

        Task CleanOutdatedChunksAsync();
    }
}
