using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RewriteMe.DataAccess.DataAdapters;
using RewriteMe.Domain.Interfaces.Repositories;
using RewriteMe.Domain.Transcription;

namespace RewriteMe.DataAccess.Repositories
{
    public class AudioSourceRepository : IAudioSourceRepository
    {
        private readonly IDbContextFactory _contextFactory;

        public AudioSourceRepository(IDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public byte[] GetAudioSource(Guid fileItemId)
        {
            using (var context = _contextFactory.Create())
            {
                return context.AudioSources
                    .Where(x => x.FileItemId == fileItemId)
                    .Select(x => x.WavSource)
                    .FirstOrDefault();
            }
        }

        public async Task<TimeSpan> GetTotalTime(Guid fileItemId)
        {
            using (var context = _contextFactory.Create())
            {
                return await context.AudioSources
                    .Where(x => x.FileItemId == fileItemId)
                    .Select(x => x.TotalTime)
                    .FirstOrDefaultAsync()
                    .ConfigureAwait(false);
            }
        }

        public async Task<int> GetLastVersion(Guid userId)
        {
            using (var context = _contextFactory.Create())
            {
                return await context.AudioSources
                    .Where(x => x.FileItem.UserId == userId)
                    .OrderByDescending(x => x.Version)
                    .Select(x => x.Version)
                    .FirstOrDefaultAsync()
                    .ConfigureAwait(false);
            }
        }

        public async Task AddAsync(AudioSource audioSource)
        {
            using (var context = _contextFactory.Create())
            {
                await context.AudioSources.AddAsync(audioSource.ToAudioSourceEntity()).ConfigureAwait(false);
                await context.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        public async Task UpdateAsync(AudioSource audioSource)
        {
            using (var context = _contextFactory.Create())
            {
                var audioSourceId = await context.AudioSources
                    .Where(x => x.FileItemId == audioSource.FileItemId)
                    .Select(x => x.Id)
                    .SingleAsync()
                    .ConfigureAwait(false);

                var entity = audioSource.ToAudioSourceEntity();
                entity.Id = audioSourceId;

                context.Attach(entity);
                context.Entry(entity).State = EntityState.Modified;

                await context.SaveChangesAsync().ConfigureAwait(false);
            }
        }
    }
}
