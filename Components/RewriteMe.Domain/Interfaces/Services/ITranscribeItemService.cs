using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RewriteMe.Domain.Transcription;

namespace RewriteMe.Domain.Interfaces.Services
{
    public interface ITranscribeItemService
    {
        Task AddAsync(IEnumerable<TranscribeItem> transcribeItem);

        Task<TranscribeItem> Get(Guid transcribeItemId);

        Task<IEnumerable<TranscribeItem>> GetAll(Guid fileItemId);

        Task<int> GetLastVersion(Guid userId);

        Task UpdateUserTranscript(Guid transcribeItemId, string transcript, int version);
    }
}
