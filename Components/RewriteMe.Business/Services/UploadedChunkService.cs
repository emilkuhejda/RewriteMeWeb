using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using RewriteMe.Domain.Interfaces.Repositories;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.Domain.Transcription;

namespace RewriteMe.Business.Services
{
    public class UploadedChunkService : IUploadedChunkService
    {
        private readonly IUploadedChunkRepository _uploadedChunkRepository;

        public UploadedChunkService(IUploadedChunkRepository uploadedChunkRepository)
        {
            _uploadedChunkRepository = uploadedChunkRepository;
        }

        public async Task AddAsync(UploadedChunk uploadedChunk)
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
    }
}
