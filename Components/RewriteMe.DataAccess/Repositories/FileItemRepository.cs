using System;
using System.Collections.Generic;
using RewriteMe.Domain.Interfaces.Repositories;
using RewriteMe.Domain.Transcription;

namespace RewriteMe.DataAccess.Repositories
{
    public class FileItemRepository : IFileItemRepository
    {
        public IEnumerable<FileItem> GetAll(Guid userId)
        {
            throw new NotImplementedException();
        }

        public FileItem GetFileItem(Guid userId, Guid fileId)
        {
            throw new NotImplementedException();
        }

        public void Add(FileItem fileItem)
        {
            throw new NotImplementedException();
        }
    }
}
