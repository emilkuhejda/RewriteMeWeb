using System;
using System.Collections.Generic;
using RewriteMe.Domain.Interfaces.Repositories;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.Domain.Transcription;

namespace RewriteMe.Business.Services
{
    public class FileItemService : IFileItemService
    {
        private readonly IFileItemRepository _fileItemRepository;

        public FileItemService(IFileItemRepository fileItemRepository)
        {
            _fileItemRepository = fileItemRepository;
        }

        public IEnumerable<FileItem> GetAll(Guid userId)
        {
            return _fileItemRepository.GetAll(userId);
        }

        public FileItem GetFileItem(Guid userId, Guid fileId)
        {
            return _fileItemRepository.GetFileItem(userId, fileId);
        }

        public void Add(FileItem fileItem)
        {
            _fileItemRepository.Add(fileItem);
        }
    }
}
