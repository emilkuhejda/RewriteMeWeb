﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RewriteMe.Domain.Messages;

namespace RewriteMe.Domain.Interfaces.Repositories
{
    public interface IInformationMessageRepository
    {
        Task AddAsync(IEnumerable<InformationMessage> informationMessages);

        Task<IEnumerable<InformationMessage>> GetAllAsync(DateTime updatedAfter);

        Task<DateTime> GetLastUpdateAsync();
    }
}