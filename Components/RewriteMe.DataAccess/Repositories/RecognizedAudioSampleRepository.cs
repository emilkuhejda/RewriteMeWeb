using System.Threading.Tasks;
using RewriteMe.DataAccess.DataAdapters;
using RewriteMe.Domain.Interfaces.Repositories;
using RewriteMe.Domain.Recording;

namespace RewriteMe.DataAccess.Repositories
{
    public class RecognizedAudioSampleRepository : IRecognizedAudioSampleRepository
    {
        private readonly IDbContextFactory _contextFactory;

        public RecognizedAudioSampleRepository(IDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task AddAsync(RecognizedAudioSample recognizedAudioSample)
        {
            using (var context = _contextFactory.Create())
            {
                await context.RecognizedAudioSamples.AddAsync(recognizedAudioSample.ToRecognizedAudioSampleEntity()).ConfigureAwait(false);
                await context.SaveChangesAsync().ConfigureAwait(false);
            }
        }
    }
}
