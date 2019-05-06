using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RewriteMe.Domain.Transcription;

namespace RewriteMe.Domain.Interfaces.Repositories
{
    public interface ITranscribeItemRepository
    {
        Task<TranscribeItem> GetAsync(Guid transcribeItemId);

        Task<IEnumerable<TranscribeItem>> GetAllAsync(Guid userId, DateTime updatedAfter, Guid applicationId);

        Task<DateTime> GetLastUpdateAsync(Guid userId);

        Task AddAsync(IEnumerable<TranscribeItem> transcribeItem);

        Task UpdateUserTranscriptAsync(Guid transcribeItemId, string transcript, DateTime dateUpdated, Guid applicationId);
    }
}
