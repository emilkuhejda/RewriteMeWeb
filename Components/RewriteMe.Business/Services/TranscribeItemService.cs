using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RewriteMe.Domain.Interfaces.Repositories;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.Domain.Transcription;

namespace RewriteMe.Business.Services
{
    public class TranscribeItemService : ITranscribeItemService
    {
        private readonly ITranscribeItemRepository _transcribeItemRepository;

        public TranscribeItemService(ITranscribeItemRepository transcribeItemRepository)
        {
            _transcribeItemRepository = transcribeItemRepository;
        }

        public async Task<TranscribeItem> GetAsync(Guid transcribeItemId)
        {
            return await _transcribeItemRepository.GetAsync(transcribeItemId).ConfigureAwait(false);
        }

        public async Task<IEnumerable<TranscribeItem>> GetAllAsync(Guid userId, DateTime updatedAfter, Guid applicationId)
        {
            return await _transcribeItemRepository.GetAllAsync(userId, updatedAfter, applicationId).ConfigureAwait(false);
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
