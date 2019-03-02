using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RewriteMe.DataAccess.DataAdapters;
using RewriteMe.DataAccess.Entities;
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
                        x.DateCreated
                    })
                    .ToListAsync()
                    .ConfigureAwait(false);

                return fileItems.Select(x => new FileItem
                {
                    Id = x.Id,
                    UserId = x.UserId,
                    Name = x.Name,
                    FileName = x.FileName,
                    DateCreated = x.DateCreated
                });
            }
        }

        public async Task<FileItem> GetFileItemAsync(Guid userId, Guid fileId)
        {
            using (var context = _contextFactory.Create())
            {
                var fileItem = await context.FileItems.FirstOrDefaultAsync(x => x.Id == fileId && x.UserId == userId).ConfigureAwait(false);
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

        public async Task RemoveAsync(Guid userId, Guid fileId)
        {
            using (var context = _contextFactory.Create())
            {
                var fileItemEntity = new FileItemEntity
                {
                    Id = fileId,
                    UserId = userId
                };

                context.Entry(fileItemEntity).State = EntityState.Deleted;
                await context.SaveChangesAsync().ConfigureAwait(false);
            }
        }
    }
}
