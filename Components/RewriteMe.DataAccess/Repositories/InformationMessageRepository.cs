﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RewriteMe.DataAccess.DataAdapters;
using RewriteMe.Domain.Interfaces.Repositories;
using RewriteMe.Domain.Messages;

namespace RewriteMe.DataAccess.Repositories
{
    public class InformationMessageRepository : IInformationMessageRepository
    {
        private readonly IDbContextFactory _contextFactory;

        public InformationMessageRepository(IDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task AddAsync(InformationMessage informationMessage)
        {
            using (var context = _contextFactory.Create())
            {
                await context.InformationMessages.AddRangeAsync(informationMessage.ToInformationMessageEntity()).ConfigureAwait(false);
                await context.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        public async Task<InformationMessage> GetAsync(Guid informationMessageId)
        {
            using (var context = _contextFactory.Create())
            {
                var entity = await context.InformationMessages
                    .Where(x => x.Id == informationMessageId)
                    .Include(x => x.LanguageVersions)
                    .AsNoTracking()
                    .SingleOrDefaultAsync()
                    .ConfigureAwait(false);

                return entity?.ToInformationMessage();
            }
        }

        public async Task<IEnumerable<InformationMessage>> GetAllAsync(Guid userId, DateTime updatedAfter)
        {
            using (var context = _contextFactory.Create())
            {
                var entities = await context.InformationMessages
                    .Include(x => x.LanguageVersions)
                    .AsNoTracking()
                    .OrderByDescending(x => x.DateUpdatedUtc)
                    .Where(x => (!x.UserId.HasValue || x.UserId.Value == userId) &&
                                ((x.DatePublishedUtc.HasValue && x.DatePublishedUtc >= updatedAfter) ||
                                 (x.DateUpdatedUtc.HasValue && x.DateUpdatedUtc >= updatedAfter)))
                    .ToListAsync()
                    .ConfigureAwait(false);

                return entities.Select(x => x.ToInformationMessage());
            }
        }

        public async Task<IEnumerable<InformationMessage>> GetAllShallowAsync()
        {
            using (var context = _contextFactory.Create())
            {
                var entities = await context.InformationMessages
                    .Where(x => !x.UserId.HasValue)
                    .AsNoTracking()
                    .ToListAsync()
                    .ConfigureAwait(false);

                return entities.Select(x => x.ToInformationMessage());
            }
        }

        public async Task UpdateAsync(InformationMessage informationMessage)
        {
            using (var context = _contextFactory.Create())
            {
                var entity = await context.InformationMessages
                    .Include(x => x.LanguageVersions)
                    .SingleOrDefaultAsync(x => x.Id == informationMessage.Id)
                    .ConfigureAwait(false);

                if (entity == null)
                    return;

                entity.CampaignName = informationMessage.CampaignName;
                entity.DateUpdatedUtc = DateTime.UtcNow;
                entity.LanguageVersions = informationMessage.LanguageVersions.Select(x => x.ToLanguageVersionEntity()).ToList();
                await context.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        public async Task UpdateDatePublishedAsync(Guid informationMessageId, DateTime datePublished)
        {
            using (var context = _contextFactory.Create())
            {
                var entity = await context.InformationMessages.SingleOrDefaultAsync(x => x.Id == informationMessageId).ConfigureAwait(false);
                if (entity == null)
                    return;

                if (entity.DatePublishedUtc.HasValue)
                    return;

                entity.DatePublishedUtc = datePublished;
                entity.DateUpdatedUtc = datePublished;
                await context.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        public async Task<InformationMessage> MarkAsOpenedAsync(Guid userId, Guid informationMessageId)
        {
            using (var context = _contextFactory.Create())
            {
                var entity = await context.InformationMessages.SingleOrDefaultAsync(x => x.Id == informationMessageId && x.UserId == userId).ConfigureAwait(false);
                if (entity == null)
                    return null;

                entity.DateUpdatedUtc = DateTime.UtcNow;
                entity.WasOpened = true;

                await context.SaveChangesAsync().ConfigureAwait(false);

                return entity.ToInformationMessage();
            }
        }

        public async Task MarkAsOpenedAsync(Guid userId, IEnumerable<Guid> ids)
        {
            using (var context = _contextFactory.Create())
            {
                foreach (var id in ids)
                {
                    var entity = await context.InformationMessages.SingleOrDefaultAsync(x => x.Id == id && x.UserId == userId).ConfigureAwait(false);
                    if (entity == null)
                        continue;

                    entity.DateUpdatedUtc = DateTime.UtcNow;
                    entity.WasOpened = true;
                }

                await context.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        public async Task<bool> CanUpdateAsync(Guid informationMessageId)
        {
            using (var context = _contextFactory.Create())
            {
                return await context.LanguageVersions.AllAsync(x => !x.SentOnAndroid && !x.SentOnOsx).ConfigureAwait(false);
            }
        }

        public async Task<DateTime> GetLastUpdateAsync(Guid userId)
        {
            using (var context = _contextFactory.Create())
            {
                return await context.InformationMessages
                           .Where(x => (!x.UserId.HasValue || x.UserId.Value == userId) &&
                                       (x.DatePublishedUtc.HasValue || x.DateUpdatedUtc.HasValue))
                           .OrderByDescending(x => x.DateUpdatedUtc)
                           .Select(x => x.DateUpdatedUtc)
                           .FirstOrDefaultAsync()
                           .ConfigureAwait(false) ?? DateTime.MinValue;
            }
        }
    }
}
