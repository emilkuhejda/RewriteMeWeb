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

        public async Task<TranscribeItem> Get(Guid transcribeItemId)
        {
            return await _transcribeItemRepository.Get(transcribeItemId).ConfigureAwait(false);
        }

        public async Task<IEnumerable<TranscribeItem>> GetAll(Guid fileItemId, int minimumVersion)
        {
            return await _transcribeItemRepository.GetAll(fileItemId, minimumVersion).ConfigureAwait(false);
        }

        public async Task<int> GetLastVersion(Guid userId)
        {
            return await _transcribeItemRepository.GetLastVersion(userId).ConfigureAwait(false);
        }

        public async Task AddAsync(IEnumerable<TranscribeItem> transcribeItem)
        {
            await _transcribeItemRepository.AddAsync(transcribeItem).ConfigureAwait(false);
        }

        public async Task UpdateUserTranscript(Guid transcribeItemId, string transcript, int version)
        {
            await _transcribeItemRepository.UpdateUserTranscript(transcribeItemId, transcript, version).ConfigureAwait(false);
        }
    }
}
