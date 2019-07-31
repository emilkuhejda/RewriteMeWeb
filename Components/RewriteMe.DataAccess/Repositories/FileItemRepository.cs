using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RewriteMe.DataAccess.DataAdapters;
using RewriteMe.Domain.Enums;
using RewriteMe.Domain.Interfaces.Repositories;
using RewriteMe.Domain.Settings;
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
                return await context.FileItems
                    .Where(x => !x.IsDeleted)
                    .AnyAsync(x => x.UserId == userId && x.Id == fileItemId)
                    .ConfigureAwait(false);
            }
        }

        public async Task<IEnumerable<FileItem>> GetAllAsync(Guid userId, DateTime updatedAfter, Guid applicationId)
        {
            using (var context = _contextFactory.Create())
            {
                var fileItems = await context.FileItems
                    .Where(x => !x.IsDeleted)
                    .Where(x => x.UserId == userId && x.DateUpdated >= updatedAfter && x.ApplicationId != applicationId)
                    .AsNoTracking()
                    .ToListAsync()
                    .ConfigureAwait(false);

                return fileItems.Select(x => x.ToFileItem());
            }
        }

        public async Task<IEnumerable<Guid>> GetAllDeletedIdsAsync(Guid userId, DateTime updatedAfter, Guid applicationId)
        {
            using (var context = _contextFactory.Create())
            {
                return await context.FileItems
                    .Where(x => x.IsDeleted)
                    .Where(x => x.UserId == userId && x.DateUpdated >= updatedAfter && x.ApplicationId != applicationId)
                    .AsNoTracking()
                    .Select(x => x.Id)
                    .ToListAsync()
                    .ConfigureAwait(false);
            }
        }

        public async Task<FileItem> GetAsync(Guid fileItemId)
        {
            using (var context = _contextFactory.Create())
            {
                var fileItem = await context.FileItems
                    .Where(x => !x.IsDeleted)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == fileItemId)
                    .ConfigureAwait(false);

                return fileItem?.ToFileItem();
            }
        }

        public async Task<FileItem> GetAsync(Guid userId, Guid fileItemId)
        {
            using (var context = _contextFactory.Create())
            {
                var fileItem = await context.FileItems
                    .Where(x => !x.IsDeleted)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == fileItemId && x.UserId == userId)
                    .ConfigureAwait(false);

                return fileItem?.ToFileItem();
            }
        }

        public async Task<TimeSpan> GetDeletedFileItemsTotalTime(Guid userId)
        {
            using (var context = _contextFactory.Create())
            {
                var totalTicks = await context.FileItems
                    .Where(x => x.IsDeleted)
                    .Where(x => x.UserId == userId)
                    .Where(x => x.RecognitionState > RecognitionState.Prepared)
                    .AsNoTracking()
                    .Select(x => x.TotalTime)
                    .SumAsync(x => x.Ticks)
                    .ConfigureAwait(false);

                return TimeSpan.FromTicks(totalTicks);
            }
        }

        public async Task<DateTime> GetLastUpdateAsync(Guid userId)
        {
            using (var context = _contextFactory.Create())
            {
                return await context.FileItems
                    .Where(x => !x.IsDeleted)
                    .Where(x => x.UserId == userId)
                    .OrderByDescending(x => x.DateUpdated)
                    .AsNoTracking()
                    .Select(x => x.DateUpdated)
                    .FirstOrDefaultAsync()
                    .ConfigureAwait(false);
            }
        }

        public async Task<DateTime> GetDeletedLastUpdateAsync(Guid userId)
        {
            using (var context = _contextFactory.Create())
            {
                return await context.FileItems
                    .Where(x => x.IsDeleted)
                    .Where(x => x.UserId == userId)
                    .OrderByDescending(x => x.DateUpdated)
                    .AsNoTracking()
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

        public async Task DeleteAsync(Guid userId, Guid fileItemId, Guid applicationId)
        {
            using (var context = _contextFactory.Create())
            {
                var entity = await context.FileItems.Where(x => !x.IsDeleted).FirstOrDefaultAsync(x => x.Id == fileItemId && x.UserId == userId).ConfigureAwait(false);
                if (entity == null)
                    return;

                entity.ApplicationId = applicationId;
                entity.DateUpdated = DateTime.UtcNow;
                entity.IsDeleted = true;

                await context.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        public async Task DeleteAllAsync(Guid userId, IEnumerable<DeletedFileItem> fileItems, Guid applicationId)
        {
            var deletedFileItems = fileItems.ToList();
            using (var context = _contextFactory.Create())
            {
                var fileItemIds = deletedFileItems.Select(x => x.Id);
                var entities = await context.FileItems
                    .Where(x => !x.IsDeleted)
                    .Where(x => fileItemIds.Contains(x.Id) && x.UserId == userId)
                    .ToListAsync()
                    .ConfigureAwait(false);

                if (!entities.Any())
                    return;

                foreach (var entity in entities)
                {
                    var deletedFileItem = deletedFileItems.Single(x => x.Id == entity.Id);
                    if (deletedFileItem.DeletedDate < entity.DateUpdated)
                        continue;

                    entity.ApplicationId = applicationId;
                    entity.DateUpdated = DateTime.UtcNow;
                    entity.IsDeleted = true;
                }

                await context.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        public async Task UpdateLanguageAsync(Guid fileItemId, string language, Guid applicationId)
        {
            using (var context = _contextFactory.Create())
            {
                var entity = await context.FileItems.Where(x => !x.IsDeleted).FirstOrDefaultAsync(x => x.Id == fileItemId).ConfigureAwait(false);
                if (entity == null)
                    return;

                entity.ApplicationId = applicationId;
                entity.Language = language;
                entity.DateUpdated = DateTime.UtcNow;

                await context.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        public async Task UpdateAsync(FileItem fileItem)
        {
            using (var context = _contextFactory.Create())
            {
                var entity = await context.FileItems.Where(x => !x.IsDeleted).FirstOrDefaultAsync(x => x.Id == fileItem.Id && x.UserId == fileItem.UserId).ConfigureAwait(false);
                if (entity == null)
                    return;

                entity.ApplicationId = fileItem.ApplicationId;
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

        public async Task UpdateSourceFileNameAsync(Guid fileItemId, string sourceFileName)
        {
            using (var context = _contextFactory.Create())
            {
                var entity = await context.FileItems.FirstOrDefaultAsync(x => x.Id == fileItemId).ConfigureAwait(false);
                if (entity == null)
                    return;

                entity.SourceFileName = sourceFileName;
                await context.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        public async Task UpdateRecognitionStateAsync(Guid fileItemId, RecognitionState recognitionState, Guid applicationId)
        {
            using (var context = _contextFactory.Create())
            {
                var fileItemEntity = await context.FileItems.Where(x => !x.IsDeleted).SingleOrDefaultAsync(x => x.Id == fileItemId).ConfigureAwait(false);
                if (fileItemEntity == null)
                    return;

                fileItemEntity.ApplicationId = applicationId;
                fileItemEntity.RecognitionState = recognitionState;
                fileItemEntity.DateUpdated = DateTime.UtcNow;

                await context.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        public async Task UpdateDateProcessedAsync(Guid fileItemId, Guid applicationId)
        {
            using (var context = _contextFactory.Create())
            {
                var fileItemEntity = await context.FileItems.Where(x => !x.IsDeleted).SingleOrDefaultAsync(x => x.Id == fileItemId).ConfigureAwait(false);
                if (fileItemEntity == null)
                    return;

                fileItemEntity.ApplicationId = applicationId;
                fileItemEntity.DateProcessed = DateTime.UtcNow;
                fileItemEntity.DateUpdated = DateTime.UtcNow;

                await context.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        public async Task<TimeSpan> GetTranscribedTotalSeconds(Guid userId)
        {
            using (var context = _contextFactory.Create())
            {
                var totalTicks = await context.FileItems
                    .Where(x => x.UserId == userId)
                    .Where(x => x.RecognitionState > RecognitionState.Prepared)
                    .AsNoTracking()
                    .Select(x => x.TotalTime)
                    .SumAsync(x => x.Ticks)
                    .ConfigureAwait(false);

                return TimeSpan.FromTicks(totalTicks);
            }
        }
    }
}
