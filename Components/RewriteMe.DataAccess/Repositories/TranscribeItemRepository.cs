using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RewriteMe.DataAccess.DataAdapters;
using RewriteMe.Domain.Enums;
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

        public async Task<TranscribeItem> GetAsync(Guid transcribeItemId)
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

        public async Task<IEnumerable<TranscribeItem>> GetAllAsync(Guid fileItemId)
        {
            using (var context = _contextFactory.Create())
            {
                var transcribeItemEntities = await context.TranscribeItems
                    .Where(x => x.FileItemId == fileItemId)
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
                        DateCreated = x.DateCreatedUtc,
                        DateUpdated = x.DateUpdatedUtc
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
                    DateCreatedUtc = x.DateCreated,
                    DateUpdatedUtc = x.DateUpdated
                });
            }
        }

        public async Task<IEnumerable<TranscribeItem>> GetAllForUserAsync(Guid userId, DateTime updatedAfter, Guid applicationId)
        {
            using (var context = _contextFactory.Create())
            {
                var transcribeItemEntities = await context.TranscribeItems
                    .Where(x => x.FileItem.UserId == userId && x.DateUpdatedUtc >= updatedAfter && x.ApplicationId != applicationId)
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
                        DateCreated = x.DateCreatedUtc,
                        DateUpdated = x.DateUpdatedUtc
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
                    DateCreatedUtc = x.DateCreated,
                    DateUpdatedUtc = x.DateUpdated
                });
            }
        }

        public async Task<DateTime> GetLastUpdateAsync(Guid userId)
        {
            using (var context = _contextFactory.Create())
            {
                return await context.TranscribeItems
                    .Where(x => x.FileItem.UserId == userId)
                    .OrderByDescending(x => x.DateUpdatedUtc)
                    .Select(x => x.DateUpdatedUtc)
                    .FirstOrDefaultAsync()
                    .ConfigureAwait(false);
            }
        }

        public async Task AddAsync(IEnumerable<TranscribeItem> transcribeItem)
        {
            using (var context = _contextFactory.Create())
            {
                await context.TranscribeItems.AddRangeAsync(transcribeItem.Select(x => x.ToTranscribeItemEntity())).ConfigureAwait(false);
                await context.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        public async Task UpdateUserTranscriptAsync(Guid transcribeItemId, string transcript, DateTime dateUpdated, Guid applicationId)
        {
            using (var context = _contextFactory.Create())
            {
                var transcribeItemEntity = await context.TranscribeItems.SingleOrDefaultAsync(x => x.Id == transcribeItemId).ConfigureAwait(false);
                if (transcribeItemEntity == null)
                    return;

                transcribeItemEntity.ApplicationId = applicationId;
                transcribeItemEntity.UserTranscript = transcript;
                transcribeItemEntity.DateUpdatedUtc = dateUpdated;
                await context.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        public async Task UpdateStorageAsync(Guid fileItemId, StorageSetting storageSetting)
        {
            using (var context = _contextFactory.Create())
            {
                var transcribeItemEntities = await context.TranscribeItems
                    .Where(x => x.FileItemId == fileItemId)
                    .ToListAsync()
                    .ConfigureAwait(false);

                if (!transcribeItemEntities.Any())
                    return;

                foreach (var transcribeItemEntity in transcribeItemEntities)
                {
                    transcribeItemEntity.Storage = storageSetting;
                }

                await context.SaveChangesAsync().ConfigureAwait(false);
            }
        }
    }
}
