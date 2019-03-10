﻿using System;
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

        public async Task<IEnumerable<FileItem>> GetAllAsync(Guid userId)
        {
            return await _fileItemRepository.GetAllAsync(userId).ConfigureAwait(false);
        }

        public async Task<FileItem> GetFileItemWithoutSourceAsync(Guid userId, Guid fileItemId)
        {
            return await _fileItemRepository.GetFileItemWithoutSourceAsync(userId, fileItemId).ConfigureAwait(false);
        }

        public async Task<FileItem> GetFileItemAsync(Guid userId, Guid fileItemId)
        {
            return await _fileItemRepository.GetFileItemAsync(userId, fileItemId).ConfigureAwait(false);
        }

        public async Task AddAsync(FileItem fileItem)
        {
            await _fileItemRepository.AddAsync(fileItem).ConfigureAwait(false);
        }

        public async Task RemoveAsync(Guid userId, Guid fileItemId)
        {
            await _fileItemRepository.RemoveAsync(userId, fileItemId).ConfigureAwait(false);
        }

        public async Task UpdateAsync(FileItem fileItem)
        {
            await _fileItemRepository.UpdateAsync(fileItem).ConfigureAwait(false);
        }

        public async Task UpdateRecognitionStateAsync(Guid fileItemId, RecognitionState recognitionState)
        {
            await _fileItemRepository.UpdateRecognitionStateAsync(fileItemId, recognitionState);
        }

        public async Task UpdateDateProcessedAsync(Guid fileItemId)
        {
            await _fileItemRepository.UpdateDateProcessedAsync(fileItemId);
        }
    }
}
