using System;
using System.Collections.Generic;
using System.Linq;
using RewriteMe.DataAccess.DataAdapters;
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

        public IEnumerable<FileItem> GetAll(Guid userId)
        {
            using (var context = _contextFactory.Create())
            {
                return context.FileItems
                    .Where(x => x.UserId == userId)
                    .Select(x => new
                    {
                        x.Id,
                        x.UserId,
                        x.Name,
                        x.FileName,
                        x.DateCreated
                    })
                    .ToList()
                    .Select(x => new FileItem
                    {
                        Id = x.Id,
                        UserId = x.UserId,
                        Name = x.Name,
                        FileName = x.FileName,
                        DateCreated = x.DateCreated
                    });
            }
        }

        public FileItem GetFileItem(Guid userId, Guid fileId)
        {
            using (var context = _contextFactory.Create())
            {
                return context.FileItems.FirstOrDefault(x => x.Id == fileId && x.UserId == userId)?.ToFileItem();
            }
        }

        public void Add(FileItem fileItem)
        {
            using (var context = _contextFactory.Create())
            {
                context.FileItems.Add(fileItem.ToFileItemEntity());
                context.SaveChanges();
            }
        }
    }
}
