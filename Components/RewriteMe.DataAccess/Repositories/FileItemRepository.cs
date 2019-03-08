using System;
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

        public async Task<IEnumerable<FileItem>> GetAllAsync(Guid userId)
        {
            using (var context = _contextFactory.Create())
            {
                var fileItems = await context.FileItems
                    .Where(x => x.UserId == userId)
                    .Select(x => new
                    {
                        x.Id,
                        x.UserId,
                        x.Name,
                        x.FileName,
                        x.ContentType,
                        x.RecognitionState,
                        x.DateCreated,
                        x.DateProcessed
                    })
                    .ToListAsync()
                    .ConfigureAwait(false);

                return fileItems.Select(x => new FileItem
                {
                    Id = x.Id,
                    UserId = x.UserId,
                    Name = x.Name,
                    FileName = x.FileName,
                    ContentType = x.ContentType,
                    RecognitionState = x.RecognitionState,
                    DateCreated = x.DateCreated,
                    DateProcessed = x.DateProcessed
                });
            }
        }

        public async Task<FileItem> GetFileItemWithoutSourceAsync(Guid userId, Guid fileItemId)
        {
            using (var context = _contextFactory.Create())
            {
                var fileItem = await context.FileItems
                    .Where(x => x.UserId == userId && x.Id == fileItemId)
                    .Select(x => new
                    {
                        x.Id,
                        x.UserId,
                        x.Name,
                        x.FileName,
                        x.ContentType,
                        x.RecognitionState,
                        x.DateCreated,
                        x.DateProcessed
                    })
                    .FirstOrDefaultAsync()
                    .ConfigureAwait(false);

                return new FileItem
                {
                    Id = fileItem.Id,
                    UserId = fileItem.UserId,
                    Name = fileItem.Name,
                    FileName = fileItem.FileName,
                    ContentType = fileItem.ContentType,
                    RecognitionState = fileItem.RecognitionState,
                    DateCreated = fileItem.DateCreated,
                    DateProcessed = fileItem.DateProcessed
                };
            }
        }

        public async Task<FileItem> GetFileItemAsync(Guid userId, Guid fileItemId)
        {
            using (var context = _contextFactory.Create())
            {
                var fileItem = await context.FileItems.FirstOrDefaultAsync(x => x.Id == fileItemId && x.UserId == userId).ConfigureAwait(false);
                return fileItem?.ToFileItem();
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

        public async Task UpdateRecognitionStateAsync(Guid fileItemId, RecognitionState recognitionState)
        {
            using (var context = _contextFactory.Create())
            {
                var fileItemEntity = await context.FileItems.SingleOrDefaultAsync(x => x.Id == fileItemId).ConfigureAwait(false);
                if (fileItemEntity == null)
                    return;

                fileItemEntity.RecognitionState = recognitionState;
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
