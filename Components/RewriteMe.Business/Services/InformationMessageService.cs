﻿using System;
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

        public async Task<IEnumerable<InformationMessage>> GetAllAsync(DateTime updatedAfter)
        {
            return await _informationMessageRepository.GetAllAsync(updatedAfter).ConfigureAwait(false);
        }
    }
}
