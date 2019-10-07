using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RewriteMe.Domain.Transcription;

namespace RewriteMe.Domain.Interfaces.Repositories
{
    public interface ITranscribeItemSourceRepository
    {
        Task<TranscribeItemSource> GetAsync(Guid transcribeItemId);

        Task AddAsync(IEnumerable<TranscribeItemSource> transcribeItemSources);
    }
}
