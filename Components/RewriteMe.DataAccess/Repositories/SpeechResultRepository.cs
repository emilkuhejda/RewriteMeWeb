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
    }
}
