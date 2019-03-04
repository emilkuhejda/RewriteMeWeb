using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RewriteMe.DataAccess.DataAdapters;
using RewriteMe.Domain.Interfaces.Repositories;
using RewriteMe.Domain.Transcription;

namespace RewriteMe.DataAccess.Repositories
{
    public class TranscribeItemRepository : ITranscribeItemRepository
    {
        private readonly IDbContextFactory _contextFactory;

        public TranscribeItemRepository(IDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task AddAsync(IEnumerable<TranscribeItem> transcribeItem)
        {
            using (var context = _contextFactory.Create())
            {
                await context.TranscribeItems.AddRangeAsync(transcribeItem.Select(x => x.ToTranscribeItemEntity())).ConfigureAwait(false);
                await context.SaveChangesAsync();
            }
        }
    }
}
