using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using RewriteMe.Domain.Interfaces.Repositories;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.Domain.Settings;
using RewriteMe.Domain.Transcription;

namespace RewriteMe.Business.Services
{
    public class TranscribeItemService : ITranscribeItemService
    {
        private readonly IFileAccessService _fileAccessService;
        private readonly ITranscribeItemRepository _transcribeItemRepository;
        private readonly ITranscribeItemSourceRepository _transcribeItemSourceRepository;
        private readonly AppSettings _appSettings;

        public TranscribeItemService(
            IFileAccessService fileAccessService,
            ITranscribeItemRepository transcribeItemRepository,
            ITranscribeItemSourceRepository transcribeItemSourceRepository,
            IOptions<AppSettings> options)
        {
            _fileAccessService = fileAccessService;
            _transcribeItemRepository = transcribeItemRepository;
            _transcribeItemSourceRepository = transcribeItemSourceRepository;
            _appSettings = options.Value;
        }

        public async Task<byte[]> GetSourceAsync(Guid transcribeItemId)
        {
            var transcribeItem = await GetAsync(transcribeItemId).ConfigureAwait(false);
            if (transcribeItem == null)
                return null;

            if (_appSettings.ReadSourceFromDisk)
            {
                var sourcePath = _fileAccessService.GetTranscriptionPath(transcribeItem);
                if (File.Exists(sourcePath))
                    return await File.ReadAllBytesAsync(sourcePath).ConfigureAwait(false);

                return null;
            }

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
