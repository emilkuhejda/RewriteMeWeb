﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using RewriteMe.Domain.Enums;
using RewriteMe.Domain.Interfaces.Repositories;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.Domain.Transcription;

namespace RewriteMe.Business.Services
{
    public class TranscribeItemService : ITranscribeItemService
    {
        private readonly IFileAccessService _fileAccessService;
        private readonly IStorageService _storageService;
        private readonly ITranscribeItemRepository _transcribeItemRepository;
        private readonly ITranscribeItemSourceRepository _transcribeItemSourceRepository;

        public TranscribeItemService(
            IFileAccessService fileAccessService,
            IStorageService storageService,
            ITranscribeItemRepository transcribeItemRepository,
            ITranscribeItemSourceRepository transcribeItemSourceRepository)
        {
            _fileAccessService = fileAccessService;
            _storageService = storageService;
            _transcribeItemRepository = transcribeItemRepository;
            _transcribeItemSourceRepository = transcribeItemSourceRepository;
        }

        public async Task<byte[]> GetSourceAsync(Guid userId, Guid transcribeItemId)
        {
            var transcribeItem = await GetAsync(transcribeItemId).ConfigureAwait(false);
            if (transcribeItem == null)
                return null;

            if (transcribeItem.Storage == StorageSetting.Database)
                return await GetTranscribeItemSourceAsync(transcribeItemId).ConfigureAwait(false);

            if (transcribeItem.Storage == StorageSetting.Disk)
                return await _storageService.GetTranscribeItemBytesAsync(transcribeItem, userId).ConfigureAwait(false);

            var sourcePath = _fileAccessService.GetTranscriptionPath(userId, transcribeItem);
            if (File.Exists(sourcePath))
                return await File.ReadAllBytesAsync(sourcePath).ConfigureAwait(false);

            return await GetTranscribeItemSourceAsync(transcribeItemId).ConfigureAwait(false);
        }

        private async Task<byte[]> GetTranscribeItemSourceAsync(Guid transcribeItemId)
        {
            var transcribeItemSource = await _transcribeItemSourceRepository.GetAsync(transcribeItemId).ConfigureAwait(false);
            return transcribeItemSource?.Source;
        }

        public async Task<TranscribeItem> GetAsync(Guid transcribeItemId)
        {
            return await _transcribeItemRepository.GetAsync(transcribeItemId).ConfigureAwait(false);
        }

        public async Task<IEnumerable<TranscribeItem>> GetAllAsync(Guid fileItemId)
        {
            return await _transcribeItemRepository.GetAllAsync(fileItemId).ConfigureAwait(false);
        }

        public async Task<IEnumerable<TranscribeItem>> GetAllForUserAsync(Guid userId, DateTime updatedAfter, Guid applicationId)
        {
            return await _transcribeItemRepository.GetAllForUserAsync(userId, updatedAfter, applicationId).ConfigureAwait(false);
        }

        public async Task<DateTime> GetLastUpdateAsync(Guid userId)
        {
            return await _transcribeItemRepository.GetLastUpdateAsync(userId).ConfigureAwait(false);
        }

        public async Task AddAsync(IEnumerable<TranscribeItem> transcribeItem)
        {
            await _transcribeItemRepository.AddAsync(transcribeItem).ConfigureAwait(false);
        }

        public async Task UpdateUserTranscriptAsync(Guid transcribeItemId, string transcript, DateTime dateUpdated, Guid applicationId)
        {
            await _transcribeItemRepository.UpdateUserTranscriptAsync(transcribeItemId, transcript, dateUpdated, applicationId).ConfigureAwait(false);
        }
    }
}
