using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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

        public async Task<TranscribeItemSource> GetAsync(Guid transcribeItemId)
        {
            using (var context = _contextFactory.Create())
            {
                var entity = await context.TranscribeItemSources.SingleOrDefaultAsync(x => x.Id == transcribeItemId).ConfigureAwait(false);

                return entity?.ToTranscribeItemSource();
            }
        }

        public async Task AddAsync(IEnumerable<TranscribeItemSource> transcribeItemSources)
        {
            using (var context = _contextFactory.Create())
            {
                var transcribeItems = transcribeItemSources.Select(x => x.ToTranscribeItemSourceEntity());
                await context.TranscribeItemSources.AddRangeAsync(transcribeItems).ConfigureAwait(false);
                await context.SaveChangesAsync().ConfigureAwait(false);
            }
        }
    }
}
