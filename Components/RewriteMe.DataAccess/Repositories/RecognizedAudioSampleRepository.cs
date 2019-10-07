using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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

        public async Task<TimeSpan> GetRecognizedTimeAsync(Guid userId)
        {
            using (var context = _contextFactory.Create())
            {
                var totalTicks = await context.RecognizedAudioSamples
                    .Where(x => x.UserId == userId)
                    .AsNoTracking()
                    .SelectMany(x => x.SpeechResults)
                    .Select(x => x.TotalTime)
                    .SumAsync(x => x.Ticks)
                    .ConfigureAwait(false);

                return TimeSpan.FromTicks(totalTicks);
            }
        }
    }
}
