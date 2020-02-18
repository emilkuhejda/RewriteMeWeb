using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using RewriteMe.Domain.Transcription;

namespace RewriteMe.Domain.Interfaces.Repositories
{
    public interface IUploadedChunkRepository
    {
        Task AddAsync(UploadedChunk uploadedChunk);

        Task<IEnumerable<UploadedChunk>> GetAllAsync(Guid fileItemId, Guid applicationId, CancellationToken cancellationToken);

        Task DeleteAsync(Guid fileItemId, Guid applicationId);

        Task CleanOutdatedChunksAsync(DateTime dateToCompare);
    }
}
