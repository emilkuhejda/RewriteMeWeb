using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RewriteMe.Domain.Transcription;

namespace RewriteMe.Domain.Interfaces.Repositories
{
    public interface ITranscribeItemRepository
    {
        Task AddAsync(IEnumerable<TranscribeItem> transcribeItem);

        Task<TranscribeItem> Get(Guid transcribeItemId);

        Task<IEnumerable<TranscribeItem>> GetAll(Guid fileItemId);

        Task UpdateUserTranscript(Guid transcribeItemId, string transcript);

        Task<TimeSpan> GetTranscriptTotalSeconds(Guid userId);
    }
}
