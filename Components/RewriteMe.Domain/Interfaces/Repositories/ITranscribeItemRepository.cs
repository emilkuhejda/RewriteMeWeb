using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RewriteMe.Domain.Transcription;

namespace RewriteMe.Domain.Interfaces.Repositories
{
    public interface ITranscribeItemRepository
    {
        Task<TranscribeItem> Get(Guid transcribeItemId);

        Task<IEnumerable<TranscribeItem>> GetAll(Guid fileItemId, DateTime updatedAfter);

        Task<DateTime> GetLastUpdateAsync(Guid userId);

        Task AddAsync(IEnumerable<TranscribeItem> transcribeItem);

        Task UpdateUserTranscript(Guid transcribeItemId, string transcript, DateTime dateUpdated);

        Task<TimeSpan> GetTranscriptTotalSeconds(Guid userId);
    }
}
