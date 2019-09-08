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

        public async Task<IEnumerable<InformationMessage>> GetAllAsync(Guid userId, DateTime updatedAfter, int? count)
        {
            using (var context = _contextFactory.Create())
            {
                var query = context.InformationMessages
                    .Include(x => x.LanguageVersions)
                    .AsNoTracking()
                    .OrderByDescending(x => x.DatePublished)
                    .Where(x => (!x.UserId.HasValue || x.UserId.Value == userId) && x.DatePublished.HasValue && x.DatePublished >= updatedAfter);

                if (count.HasValue)
                    query = query.Take(count.Value);

                var entities = await query.ToListAsync().ConfigureAwait(false);

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

                if (entity.DatePublished.HasValue)
                    return;

                entity.DatePublished = datePublished;
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

        public async Task<DateTime> GetLastUpdateAsync()
        {
            using (var context = _contextFactory.Create())
            {
                return await context.InformationMessages
                    .OrderByDescending(x => x.DatePublished)
                    .Select(x => x.DatePublished)
                    .FirstOrDefaultAsync()
                    .ConfigureAwait(false) ?? DateTime.MinValue;
            }
        }
    }
}
