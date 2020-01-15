using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RewriteMe.Domain.Transcription;

namespace RewriteMe.Domain.Interfaces.Services
{
    public interface IUploadedChunkService
    {
        Task AddAsync(UploadedChunk uploadedChunk);

        Task<IEnumerable<UploadedChunk>> GetAllAsync(Guid fileItemId);
    }
}
