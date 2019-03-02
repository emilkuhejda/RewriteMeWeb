using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        public async Task<IEnumerable<FileItem>> GetAllAsync(Guid userId)
        {
            return await _fileItemRepository.GetAllAsync(userId).ConfigureAwait(false);
        }

        public async Task<FileItem> GetFileItemAsync(Guid userId, Guid fileId)
        {
            return await _fileItemRepository.GetFileItemAsync(userId, fileId).ConfigureAwait(false);
        }

        public async Task AddAsync(FileItem fileItem)
        {
            await _fileItemRepository.AddAsync(fileItem).ConfigureAwait(false);
        }
    }
}
