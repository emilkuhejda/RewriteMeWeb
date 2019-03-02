using System;
using System.Collections.Generic;
using RewriteMe.Domain.Transcription;

namespace RewriteMe.Domain.Interfaces.Repositories
{
    public interface IFileItemRepository
    {
        IEnumerable<FileItem> GetAll(Guid userId);

        FileItem GetFileItem(Guid userId, Guid fileId);

        void Add(FileItem fileItem);
    }
}
