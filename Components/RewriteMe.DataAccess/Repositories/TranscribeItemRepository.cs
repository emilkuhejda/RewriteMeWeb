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

        public async Task AddAsync(IEnumerable<TranscribeItem> transcribeItem)
        {
            using (var context = _contextFactory.Create())
            {
                await context.TranscribeItems.AddRangeAsync(transcribeItem.Select(x => x.ToTranscribeItemEntity())).ConfigureAwait(false);
                await context.SaveChangesAsync();
            }
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

        public async Task<IEnumerable<TranscribeItem>> GetAll(Guid fileItemId)
        {
            using (var context = _contextFactory.Create())
            {
                var transcribeItemEntities = await context.TranscribeItems
                    .Where(x => x.FileItemId == fileItemId)
                    .AsNoTracking()
                    .Select(x => new
                    {
                        x.Id,
                        x.Alternatives,
                        x.UserTranscript,
                        x.TotalTime,
                        x.DateCreated
                    })
                    .ToListAsync();

                return transcribeItemEntities.Select(x => new TranscribeItem
                {
                    Id = x.Id,
                    Alternatives = JsonConvert.DeserializeObject<IEnumerable<RecognitionAlternative>>(x.Alternatives),
                    UserTranscript = x.UserTranscript,
                    TotalTime = x.TotalTime,
                    DateCreated = x.DateCreated
                });
            }
        }

        public async Task UpdateUserTranscript(Guid transcribeItemId, string transcript)
        {
            using (var context = _contextFactory.Create())
            {
                var transcribeItemEntity = await context.TranscribeItems.SingleOrDefaultAsync(x => x.Id == transcribeItemId).ConfigureAwait(false);
                if (transcribeItemEntity == null)
                    return;

                transcribeItemEntity.UserTranscript = transcript;
                await context.SaveChangesAsync().ConfigureAwait(false);
            }
        }
    }
}
