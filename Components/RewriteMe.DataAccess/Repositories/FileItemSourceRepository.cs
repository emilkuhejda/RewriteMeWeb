﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RewriteMe.DataAccess.DataAdapters;
using RewriteMe.Domain.Interfaces.Repositories;
using RewriteMe.Domain.Transcription;

namespace RewriteMe.DataAccess.Repositories
{
    public class FileItemSourceRepository : IFileItemSourceRepository
    {
        private readonly IDbContextFactory _contextFactory;

        public FileItemSourceRepository(IDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<FileItemSource> GetAsync(Guid fileItemId)
        {
            using (var context = _contextFactory.Create())
            {
                var entity = await context.FileItemSources.SingleOrDefaultAsync(x => x.FileItemId == fileItemId).ConfigureAwait(false);
                return entity?.ToFileItemSource();
            }
        }

        public async Task<bool> HasFileItemSourceAsync(Guid fileItemId)
        {
            using (var context = _contextFactory.Create())
            {
                var entity = await context.FileItemSources
                    .Where(x => x.FileItemId == fileItemId && x.Source != null && x.Source.Any())
                    .Select(x => new { x.Id })
                    .SingleOrDefaultAsync()
                    .ConfigureAwait(false);

                return entity != null;
            }
        }

        public async Task AddAsync(FileItemSource fileItemSource)
        {
            using (var context = _contextFactory.Create())
            {
                await context.FileItemSources.AddAsync(fileItemSource.ToFileItemSourceEntity()).ConfigureAwait(false);
                await context.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        public async Task UpdateSource(Guid fileItemId, byte[] source)
        {
            using (var context = _contextFactory.Create())
            {
                var entity = await context.FileItemSources.SingleOrDefaultAsync(x => x.FileItemId == fileItemId).ConfigureAwait(false);
                if (entity == null)
                    return;

                entity.Source = source;
                await context.SaveChangesAsync().ConfigureAwait(false);
            }
        }
    }
}
