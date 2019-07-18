using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RewriteMe.DataAccess.DataAdapters;
using RewriteMe.Domain.Interfaces.Repositories;
using RewriteMe.Domain.Recording;

namespace RewriteMe.DataAccess.Repositories
{
    public class SpeechResultRepository : ISpeechResultRepository
    {
        private readonly IDbContextFactory _contextFactory;

        public SpeechResultRepository(IDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task AddAsync(SpeechResult speechResult)
        {
            using (var context = _contextFactory.Create())
            {
                await context.SpeechResults.AddAsync(speechResult.ToSpeechResultEntity()).ConfigureAwait(false);
                await context.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        public async Task UpdateAllAsync(IEnumerable<SpeechResult> speechResults)
        {
            using (var context = _contextFactory.Create())
            {
                var entities = speechResults.Select(x => x.ToSpeechResultEntity());
                foreach (var speechResultEntity in entities)
                {
                    context.Attach(speechResultEntity);
                    context.Entry(speechResultEntity).Property(x => x.TotalTime).IsModified = true;
                }

                await context.SaveChangesAsync().ConfigureAwait(false);
            }
        }
    }
}
