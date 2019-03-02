using System;
using System.Collections.Generic;
using RewriteMe.Domain.Transcription;

namespace RewriteMe.Domain.Interfaces.Services
{
    public interface IFileItemService
    {
        IEnumerable<FileItem> GetAll(Guid userId);

        FileItem GetFileItem(Guid userId, Guid fileId);

        void Add(FileItem fileItem);
    }
}
