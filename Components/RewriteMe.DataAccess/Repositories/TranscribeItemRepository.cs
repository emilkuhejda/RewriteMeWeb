using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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

        public async Task<TranscribeItem> Get(Guid transcribeItemId)
        {
            using (var context = _contextFactory.Create())
            {
                var transcribeItemEntity = await context.TranscribeItems
                    .AsNoTracking()
                    .SingleOrDefaultAsync(x => x.Id == transcribeItemId)
                    .ConfigureAwait(false);

                return transcribeItemEntity?.ToTranscribeItem();
            }
        }

        public async Task<IEnumerable<TranscribeItem>> GetAll(Guid userId, DateTime updatedAfter)
        {
            using (var context = _contextFactory.Create())
            {
                var transcribeItemEntities = await context.TranscribeItems
                    .Where(x => x.FileItem.UserId == userId && x.DateUpdated >= updatedAfter)
                    .AsNoTracking()
                    .Select(x => new
                    {
                        x.Id,
                        x.FileItemId,
                        x.Alternatives,
                        x.UserTranscript,
                        x.StartTime,
                        x.EndTime,
                        x.TotalTime,
                        x.DateCreated,
                        x.DateUpdated
                    })
                    .OrderBy(x => x.StartTime)
                    .ToListAsync()
                    .ConfigureAwait(false);

                return transcribeItemEntities.Select(x => new TranscribeItem
                {
                    Id = x.Id,
                    FileItemId = x.FileItemId,
                    Alternatives = JsonConvert.DeserializeObject<IEnumerable<RecognitionAlternative>>(x.Alternatives),
                    UserTranscript = x.UserTranscript,
                    StartTime = x.StartTime,
                    EndTime = x.EndTime,
                    TotalTime = x.TotalTime,
                    DateCreated = x.DateCreated,
                    DateUpdated = x.DateUpdated
                });
            }
        }

        public async Task<DateTime> GetLastUpdateAsync(Guid userId)
        {
            using (var context = _contextFactory.Create())
            {
                return await context.TranscribeItems
                    .Where(x => x.FileItem.UserId == userId)
                    .OrderByDescending(x => x.DateUpdated)
                    .Select(x => x.DateUpdated)
                    .FirstOrDefaultAsync()
                    .ConfigureAwait(false);
            }
        }

        public async Task AddAsync(IEnumerable<TranscribeItem> transcribeItem)
        {
            using (var context = _contextFactory.Create())
            {
                await context.TranscribeItems.AddRangeAsync(transcribeItem.Select(x => x.ToTranscribeItemEntity())).ConfigureAwait(false);
                await context.SaveChangesAsync();
            }
        }

        public async Task UpdateUserTranscript(Guid transcribeItemId, string transcript, DateTime dateUpdated)
        {
            using (var context = _contextFactory.Create())
            {
                var transcribeItemEntity = await context.TranscribeItems.SingleOrDefaultAsync(x => x.Id == transcribeItemId).ConfigureAwait(false);
                if (transcribeItemEntity == null)
                    return;

                transcribeItemEntity.UserTranscript = transcript;
                transcribeItemEntity.DateUpdated = dateUpdated;
                await context.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        public async Task<TimeSpan> GetTranscriptTotalSeconds(Guid userId)
        {
            using (var context = _contextFactory.Create())
            {
                var totalTicks = await context.TranscribeItems
                    .Where(x => x.FileItem.UserId == userId)
                    .Select(x => x.TotalTime)
                    .SumAsync(x => x.Ticks)
                    .ConfigureAwait(false);

                return TimeSpan.FromTicks(totalTicks);
            }
        }
    }
}
