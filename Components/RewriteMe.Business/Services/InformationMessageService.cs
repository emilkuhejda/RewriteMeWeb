﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RewriteMe.Domain.Interfaces.Repositories;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.Domain.Messages;
using Serilog;

namespace RewriteMe.Business.Services
{
    public class InformationMessageService : IInformationMessageService
    {
        private readonly IInformationMessageRepository _informationMessageRepository;
        private readonly ILogger _logger;

        public InformationMessageService(
            IInformationMessageRepository informationMessageRepository,
            ILogger logger)
        {
            _informationMessageRepository = informationMessageRepository;
            _logger = logger.ForContext<InformationMessageService>();
        }

        public async Task AddAsync(InformationMessage informationMessage)
        {
            await _informationMessageRepository.AddAsync(informationMessage).ConfigureAwait(false);

            _logger.Information($"Information message '{informationMessage.Id}' was created.");
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

            _logger.Information($"Information message '{informationMessage.Id}' was updated.");
        }

        public async Task UpdateDatePublishedAsync(Guid informationMessageId, DateTime datePublished)
        {
            await _informationMessageRepository.UpdateDatePublishedAsync(informationMessageId, datePublished).ConfigureAwait(false);

            _logger.Information($"Date published for information message '{informationMessageId}' was updated.");
        }

        public async Task<InformationMessage> MarkAsOpenedAsync(Guid userId, Guid informationMessageId)
        {
            var informationMessage = await _informationMessageRepository.MarkAsOpenedAsync(userId, informationMessageId).ConfigureAwait(false);

            _logger.Information($"Information message '{informationMessage.Id}' was mark as opened.");

            return informationMessage;
        }

        public async Task MarkAsOpenedAsync(Guid userId, IEnumerable<Guid> ids)
        {
            await _informationMessageRepository.MarkAsOpenedAsync(userId, ids).ConfigureAwait(false);

            _logger.Information($"Information messages '{ids}' were mark as opened.");
        }

        public async Task<bool> CanUpdateAsync(Guid informationMessageId)
        {
            return await _informationMessageRepository.CanUpdateAsync(informationMessageId).ConfigureAwait(false);
        }

        public async Task<DateTime> GetLastUpdateAsync(Guid userId)
        {
            return await _informationMessageRepository.GetLastUpdateAsync(userId).ConfigureAwait(false);
        }
    }
}
