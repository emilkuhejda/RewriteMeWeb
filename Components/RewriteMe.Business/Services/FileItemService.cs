using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RewriteMe.Domain.Enums;
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

        public async Task<bool> ExistsAsync(Guid userId, Guid fileItemId)
        {
            return await _fileItemRepository.ExistsAsync(userId, fileItemId).ConfigureAwait(false);
        }

        public async Task<IEnumerable<FileItem>> GetAllAsync(Guid userId, DateTime updatedAfter, Guid applicationId)
        {
            return await _fileItemRepository.GetAllAsync(userId, updatedAfter, applicationId).ConfigureAwait(false);
        }

        public async Task<IEnumerable<Guid>> GetAllDeletedIdsAsync(Guid userId, DateTime updatedAfter, Guid applicationId)
        {
            return await _fileItemRepository.GetAllDeletedIdsAsync(userId, updatedAfter, applicationId).ConfigureAwait(false);
        }

        public async Task<FileItem> GetAsync(Guid userId, Guid fileItemId)
        {
            return await _fileItemRepository.GetAsync(userId, fileItemId).ConfigureAwait(false);
        }

        public async Task<DateTime> GetLastUpdateAsync(Guid userId)
        {
            return await _fileItemRepository.GetLastUpdateAsync(userId).ConfigureAwait(false);
        }

        public async Task<DateTime> GetDeletedLastUpdateAsync(Guid userId)
        {
            return await _fileItemRepository.GetLastUpdateAsync(userId).ConfigureAwait(false);
        }

        public async Task AddAsync(FileItem fileItem)
        {
            await _fileItemRepository.AddAsync(fileItem).ConfigureAwait(false);
        }

        public async Task DeleteAsync(Guid userId, Guid fileItemId)
        {
            await _fileItemRepository.DeleteAsync(userId, fileItemId).ConfigureAwait(false);
        }

        public async Task UpdateLanguageAsync(Guid fileItemId, string language, Guid applicationId)
        {
            await _fileItemRepository.UpdateLanguageAsync(fileItemId, language, applicationId).ConfigureAwait(false);
        }

        public async Task UpdateAsync(FileItem fileItem)
        {
            await _fileItemRepository.UpdateAsync(fileItem).ConfigureAwait(false);
        }

        public async Task UpdateRecognitionStateAsync(Guid fileItemId, RecognitionState recognitionState, Guid applicationId)
        {
            await _fileItemRepository.UpdateRecognitionStateAsync(fileItemId, recognitionState, applicationId).ConfigureAwait(false);
        }

        public async Task UpdateDateProcessedAsync(Guid fileItemId, Guid applicationId)
        {
            await _fileItemRepository.UpdateDateProcessedAsync(fileItemId, applicationId).ConfigureAwait(false);
        }
    }
}
