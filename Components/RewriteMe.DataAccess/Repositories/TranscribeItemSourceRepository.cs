using System.Threading.Tasks;
using RewriteMe.DataAccess.DataAdapters;
using RewriteMe.Domain.Interfaces.Repositories;
using RewriteMe.Domain.Transcription;

namespace RewriteMe.DataAccess.Repositories
{
    public class TranscribeItemSourceRepository : ITranscribeItemSourceRepository
    {
        private readonly IDbContextFactory _contextFactory;

        public TranscribeItemSourceRepository(IDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task AddAsync(TranscribeItemSource transcribeItemSource)
        {
            using (var context = _contextFactory.Create())
            {
                await context.TranscribeItemSources.AddAsync(transcribeItemSource.ToTranscribeItemSourceEntity()).ConfigureAwait(false);
                await context.SaveChangesAsync().ConfigureAwait(false);
            }
        }
    }
}
