﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RewriteMe.DataAccess.DataAdapters;
using RewriteMe.DataAccess.Entities;
using RewriteMe.DataAccess.Extensions;
using RewriteMe.Domain.Enums;
using RewriteMe.Domain.Exceptions;
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

        public async Task<bool> ExistsAsync(Guid fileItemId, string fileName)
        {
            using (var context = _contextFactory.Create())
            {
                return await context.FileItems
                    .AnyAsync(x => x.Id == fileItemId && x.FileName == fileName)
                    .ConfigureAwait(false);
            }
        }

        public async Task<IEnumerable<FileItem>> GetAllAsync(Guid userId, DateTime updatedAfter, Guid applicationId)
        {
            using (var context = _contextFactory.Create())
            {
                var fileItems = await context.FileItems
                    .Where(x => !x.IsDeleted)
                    .Where(x => x.UserId == userId && x.DateUpdatedUtc >= updatedAfter && x.ApplicationId != applicationId)
                    .AsNoTracking()
                    .ToListAsync()
                    .ConfigureAwait(false);

                return fileItems.Select(x => x.ToFileItem());
            }
        }

        public async Task<IEnumerable<FileItem>> GetAllForUserAsync(Guid userId)
        {
            using (var context = _contextFactory.Create())
            {
                var fileItems = await context.FileItems
                    .Where(x => x.UserId == userId)
                    .AsNoTracking()
                    .ToListAsync()
                    .ConfigureAwait(false);

                return fileItems.Select(x => x.ToFileItem());
            }
        }

        public async Task<IEnumerable<FileItem>> GetTemporaryDeletedFileItemsAsync(Guid userId)
        {
            using (var context = _contextFactory.Create())
            {
                var entities = await context.FileItems
                    .Where(x => x.UserId == userId)
                    .Where(x => x.IsDeleted && !x.IsPermanentlyDeleted)
                    .AsNoTracking()
                    .ToListAsync()
                    .ConfigureAwait(false);

                return entities?.Select(x => x.ToFileItem());
            }
        }

        public async Task<IEnumerable<Guid>> GetAllDeletedIdsAsync(Guid userId, DateTime updatedAfter, Guid applicationId)
        {
            using (var context = _contextFactory.Create())
            {
                return await context.FileItems
                    .Where(x => x.IsDeleted)
                    .Where(x => x.UserId == userId && x.DateUpdatedUtc >= updatedAfter && x.ApplicationId != applicationId)
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

        public async Task<FileItem> GetAsAdminAsync(Guid fileItemId)
        {
            using (var context = _contextFactory.Create())
            {
                var fileItem = await context.FileItems
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == fileItemId)
                    .ConfigureAwait(false);

                return fileItem?.ToFileItem();
            }
        }

        public async Task<DateTime> GetLastUpdateAsync(Guid userId)
        {
            using (var context = _contextFactory.Create())
            {
                return await context.FileItems
                    .Where(x => !x.IsDeleted)
                    .Where(x => x.UserId == userId)
                    .OrderByDescending(x => x.DateUpdatedUtc)
                    .AsNoTracking()
                    .Select(x => x.DateUpdatedUtc)
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
                    .OrderByDescending(x => x.DateUpdatedUtc)
                    .AsNoTracking()
                    .Select(x => x.DateUpdatedUtc)
                    .FirstOrDefaultAsync()
                    .ConfigureAwait(false);
            }
        }

        public async Task<IEnumerable<FileItem>> GetFileItemsInProgressAsync()
        {
            using (var context = _contextFactory.Create())
            {
                var entities = await context.FileItems
                    .Where(x => !x.IsDeleted && x.RecognitionState >= RecognitionState.Converting && x.RecognitionState <= RecognitionState.InProgress)
                    .AsNoTracking()
                    .ToListAsync()
                    .ConfigureAwait(false);

                return entities.Select(x => x.ToFileItem());
            }
        }

        public async Task<bool> IsInPreparedStateAsync(Guid fileItemId)
        {
            using (var context = _contextFactory.Create())
            {
                var fileItem = await context.FileItems
                    .Where(x => x.Id == fileItemId)
                    .Select(x => new
                    {
                        x.Id,
                        x.RecognitionState
                    })
                    .FirstOrDefaultAsync()
                    .ConfigureAwait(false);

                if (fileItem == null)
                    throw new FileItemNotExistsException();

                return fileItem.RecognitionState == RecognitionState.Prepared;
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
                entity.DateUpdatedUtc = DateTime.UtcNow;
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
                    if (deletedFileItem.DeletedDate < entity.DateUpdatedUtc)
                        continue;

                    entity.ApplicationId = applicationId;
                    entity.DateUpdatedUtc = DateTime.UtcNow;
                    entity.IsDeleted = true;
                }

                await context.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        public async Task PermanentDeleteAllAsync(Guid userId, IEnumerable<Guid> fileItemIds, Guid applicationId)
        {
            using (var context = _contextFactory.Create())
            {
                var entities = await context.FileItems
                    .Where(x => fileItemIds.Contains(x.Id) && x.UserId == userId)
                    .AsNoTracking()
                    .ToListAsync()
                    .ConfigureAwait(false);

                if (!entities.Any())
                    return;

                context.RemoveRange(entities);
                await context.SaveChangesAsync().ConfigureAwait(false);

                await context.FileItems.AddRangeAsync(entities.Select(x => x.CreateDeletedEntity(applicationId))).ConfigureAwait(false);
                await context.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        public async Task RestoreAllAsync(Guid userId, IEnumerable<Guid> fileItemIds, Guid applicationId)
        {
            using (var context = _contextFactory.Create())
            {
                var entities = await context.FileItems
                    .Where(x => fileItemIds.Contains(x.Id) && x.UserId == userId)
                    .ToListAsync()
                    .ConfigureAwait(false);

                if (!entities.Any())
                    return;

                foreach (var entity in entities)
                {
                    entity.ApplicationId = applicationId;
                    entity.DateUpdatedUtc = DateTime.UtcNow;
                    entity.IsDeleted = false;
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
                entity.DateUpdatedUtc = DateTime.UtcNow;

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
                entity.OriginalSourceFileName = fileItem.OriginalSourceFileName;
                entity.Storage = fileItem.Storage;
                entity.TotalTime = fileItem.TotalTime;
                entity.DateUpdatedUtc = fileItem.DateUpdatedUtc;

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
                fileItemEntity.DateUpdatedUtc = DateTime.UtcNow;

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
                fileItemEntity.DateProcessedUtc = DateTime.UtcNow;
                fileItemEntity.DateUpdatedUtc = DateTime.UtcNow;

                await context.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        public async Task UpdateTranscribedTimeAsync(Guid fileItemId, TimeSpan transcribedTime)
        {
            using (var context = _contextFactory.Create())
            {
                var fileItemEntity = await context.FileItems.Where(x => !x.IsDeleted).SingleOrDefaultAsync(x => x.Id == fileItemId).ConfigureAwait(false);
                if (fileItemEntity == null)
                    return;

                fileItemEntity.TranscribedTime = transcribedTime;
                await context.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        public async Task UpdateUploadStatusAsync(Guid fileItemId, UploadStatus uploadStatus, Guid applicationId)
        {
            using (var context = _contextFactory.Create())
            {
                var entity = await context.FileItems.SingleOrDefaultAsync(x => x.Id == fileItemId).ConfigureAwait(false);
                if (entity == null)
                    return;

                entity.ApplicationId = applicationId;
                entity.UploadStatus = uploadStatus;
                entity.DateUpdatedUtc = DateTime.UtcNow;

                await context.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        public async Task UpdateStorageAsync(Guid fileItemId, StorageSetting storageSetting)
        {
            using (var context = _contextFactory.Create())
            {
                var entity = await context.FileItems.SingleOrDefaultAsync(x => x.Id == fileItemId).ConfigureAwait(false);
                if (entity == null)
                    return;

                entity.Storage = storageSetting;

                await context.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        public async Task<bool> HasTranscribeItems(Guid fileItemId)
        {
            using (var context = _contextFactory.Create())
            {
                return await context.TranscribeItems
                    .Where(x => x.FileItemId == fileItemId)
                    .AnyAsync()
                    .ConfigureAwait(false);
            }
        }

        public async Task MarkAsCleanedAsync(Guid fileItemId)
        {
            using (var context = _contextFactory.Create())
            {
                var entity = await context.FileItems.SingleOrDefaultAsync(x => x.Id == fileItemId).ConfigureAwait(false);
                if (entity == null)
                    return;

                entity.WasCleaned = true;

                await context.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        public async Task<IEnumerable<(Guid FileItemId, Guid UserId)>> GetFileItemsForCleaningAsync(DateTime deleteBefore, bool forceCleanUp)
        {
            using (var context = _contextFactory.Create())
            {
                var query = context.FileItems
                    .Where(x => x.RecognitionState == RecognitionState.Completed)
                    .Where(x => x.DateProcessedUtc.HasValue && x.DateProcessedUtc < deleteBefore);

                if (forceCleanUp)
                {
                    query = query.Where(x => !x.WasCleaned);
                }

                var fileItems = await query
                    .Select(x => new { x.Id, x.UserId })
                    .ToListAsync()
                    .ConfigureAwait(false);

                return fileItems.Select(x => (x.Id, x.UserId));
            }
        }

        public async Task<IEnumerable<FileItem>> GetFileItemsForMigrationAsync()
        {
            using (var context = _contextFactory.Create())
            {
                var dateToCompare = DateTime.UtcNow.AddDays(-1);
                var entities = await context.FileItems
                    .Where(x => x.Storage == StorageSetting.Disk)
                    .Where(x => x.RecognitionState == RecognitionState.None ||
                                x.RecognitionState == RecognitionState.Prepared ||
                                (x.RecognitionState == RecognitionState.Completed && x.DateProcessedUtc < dateToCompare))
                    .ToListAsync()
                    .ConfigureAwait(false);

                return entities?.Select(x => x.ToFileItem());
            }
        }

        public async Task CleanSourceDataAsync(Guid fileItemId)
        {
            using (var context = _contextFactory.Create())
            {
                var fileItemDbSet = context.Set<FileItemSourceEntity>();
                fileItemDbSet.RemoveRange(fileItemDbSet.Where(x => x.FileItemId == fileItemId));

                var transcribeItemDbSet = context.Set<TranscribeItemSourceEntity>();
                transcribeItemDbSet.RemoveRange(transcribeItemDbSet.Where(x => x.FileItemId == fileItemId));

                await context.SaveChangesAsync().ConfigureAwait(false);
            }
        }
    }
}
