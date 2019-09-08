using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RewriteMe.Domain.Interfaces.Repositories;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.Domain.Messages;

namespace RewriteMe.Business.Services
{
    public class InformationMessageService : IInformationMessageService
    {
        private readonly IInformationMessageRepository _informationMessageRepository;

        public InformationMessageService(IInformationMessageRepository informationMessageRepository)
        {
            _informationMessageRepository = informationMessageRepository;
        }

        public async Task AddAsync(InformationMessage informationMessage)
        {
            await _informationMessageRepository.AddAsync(informationMessage).ConfigureAwait(false);
        }

        public async Task<InformationMessage> GetAsync(Guid informationMessageId)
        {
            return await _informationMessageRepository.GetAsync(informationMessageId).ConfigureAwait(false);
        }

        public async Task<IEnumerable<InformationMessage>> GetAllAsync(Guid userId, DateTime updatedAfter)
        {
            return await _informationMessageRepository.GetAllAsync(userId, updatedAfter).ConfigureAwait(false);
        }

        public async Task<IEnumerable<InformationMessage>> GetAllShallowAsync()
        {
            return await _informationMessageRepository.GetAllShallowAsync().ConfigureAwait(false);
        }

        public async Task UpdateAsync(InformationMessage informationMessage)
        {
            await _informationMessageRepository.UpdateAsync(informationMessage).ConfigureAwait(false);
        }

        public async Task UpdateDatePublishedAsync(Guid informationMessageId, DateTime datePublished)
        {
            await _informationMessageRepository.UpdateDatePublishedAsync(informationMessageId, datePublished).ConfigureAwait(false);
        }

        public async Task<InformationMessage> MarkAsOpened(Guid userId, Guid informationMessageId)
        {
            return await _informationMessageRepository.MarkAsOpened(userId, informationMessageId).ConfigureAwait(false);
        }

        public async Task<bool> CanUpdateAsync(Guid informationMessageId)
        {
            return await _informationMessageRepository.CanUpdateAsync(informationMessageId).ConfigureAwait(false);
        }

        public async Task<DateTime> GetLastUpdateAsync()
        {
            return await _informationMessageRepository.GetLastUpdateAsync().ConfigureAwait(false);
        }
    }
}
