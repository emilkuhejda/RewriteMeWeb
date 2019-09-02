using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RewriteMe.Domain.Messages;

namespace RewriteMe.Domain.Interfaces.Services
{
    public interface IInformationMessageService
    {
        Task AddAsync(InformationMessage informationMessage);

        Task<IEnumerable<InformationMessage>> GetAllAsync(DateTime updatedAfter);

        Task<DateTime> GetLastUpdateAsync();
    }
}
