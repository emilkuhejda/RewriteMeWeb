﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RewriteMe.DataAccess.DataAdapters;
using RewriteMe.DataAccess.Entities;
using RewriteMe.Domain.Enums;
using RewriteMe.Domain.Interfaces.Repositories;
using RewriteMe.Domain.Transcription;

namespace RewriteMe.DataAccess.Repositories
{
    public class FileItemRepository : IFileItemRepository
    {
        private readonly IDbContextFactory _contextFactory;

        public FileItemRepository(IDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<bool> ExistsAsync(Guid userId, Guid fileItemId)
        {
            using (var context = _contextFactory.Create())
            {
                return await context.FileItems.AnyAsync(x => x.UserId == userId && x.Id == fileItemId).ConfigureAwait(false);
            }
        }

        public async Task<IEnumerable<FileItem>> GetAllAsync(Guid userId, DateTime updatedAfter)
        {
            using (var context = _contextFactory.Create())
            {
                var fileItems = await context.FileItems
                    .Where(x => x.UserId == userId && x.DateUpdated >= updatedAfter)
                    .AsNoTracking()
                    .ToListAsync()
                    .ConfigureAwait(false);

                return fileItems.Select(x => x.ToFileItem());
            }
        }

        public async Task<FileItem> GetAsync(Guid userId, Guid fileItemId)
        {
            using (var context = _contextFactory.Create())
            {
                var fileItem = await context.FileItems
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == fileItemId && x.UserId == userId)
                    .ConfigureAwait(false);

                return fileItem?.ToFileItem();
            }
        }

        public FileItem Get(Guid userId, Guid fileItemId)
        {
            using (var context = _contextFactory.Create())
            {
                var fileItem = context.FileItems
                    .AsNoTracking()
                    .FirstOrDefault(x => x.Id == fileItemId && x.UserId == userId);

                return fileItem?.ToFileItem();
            }
        }

        public async Task<DateTime> GetLastUpdateAsync(Guid userId)
        {
            using (var context = _contextFactory.Create())
            {
                return await context.FileItems
                    .Where(x => x.UserId == userId)
                    .OrderByDescending(x => x.DateUpdated)
                    .Select(x => x.DateUpdated)
                    .FirstOrDefaultAsync()
                    .ConfigureAwait(false);
            }
        }

        public async Task AddAsync(FileItem fileItem)
        {
            using (var context = _contextFactory.Create())
            {
                await context.FileItems.AddAsync(fileItem.ToFileItemEntity()).ConfigureAwait(false);
                await context.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        public async Task RemoveAsync(Guid userId, Guid fileItemId)
        {
            using (var context = _contextFactory.Create())
            {
                var fileItemEntity = new FileItemEntity
                {
                    Id = fileItemId,
                    UserId = userId
                };

                context.Entry(fileItemEntity).State = EntityState.Deleted;
                await context.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        public async Task UpdateLanguageAsync(Guid fileItemId, string language)
        {
            using (var context = _contextFactory.Create())
            {
                var entity = await context.FileItems.FirstOrDefaultAsync(x => x.Id == fileItemId).ConfigureAwait(false);
                if (entity == null)
                    return;

                entity.Language = language;
                entity.DateUpdated = DateTime.UtcNow;

                await context.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        public async Task UpdateAsync(FileItem fileItem)
        {
            using (var context = _contextFactory.Create())
            {
                var entity = await context.FileItems.FirstOrDefaultAsync(x => x.Id == fileItem.Id && x.UserId == fileItem.UserId).ConfigureAwait(false);
                if (entity == null)
                    return;

                entity.Name = fileItem.Name;
                entity.Language = fileItem.Language;
                entity.DateUpdated = fileItem.DateUpdated;

                if (!string.IsNullOrEmpty(fileItem.FileName))
                {
                    entity.FileName = fileItem.FileName;
                    entity.RecognitionState = RecognitionState.None;
                }

                await context.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        public async Task UpdateRecognitionStateAsync(Guid fileItemId, RecognitionState recognitionState)
        {
            using (var context = _contextFactory.Create())
            {
                var fileItemEntity = await context.FileItems.SingleOrDefaultAsync(x => x.Id == fileItemId).ConfigureAwait(false);
                if (fileItemEntity == null)
                    return;

                fileItemEntity.RecognitionState = recognitionState;
                fileItemEntity.DateUpdated = DateTime.UtcNow;

                await context.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        public async Task UpdateDateProcessedAsync(Guid fileItemId)
        {
            using (var context = _contextFactory.Create())
            {
                var fileItemEntity = await context.FileItems.SingleOrDefaultAsync(x => x.Id == fileItemId).ConfigureAwait(false);
                if (fileItemEntity == null)
                    return;

                fileItemEntity.DateProcessed = DateTime.UtcNow;

                await context.SaveChangesAsync().ConfigureAwait(false);
            }
        }
    }
}
