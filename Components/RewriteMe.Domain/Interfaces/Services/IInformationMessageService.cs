using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RewriteMe.Domain.Messages;

namespace RewriteMe.Domain.Interfaces.Services
{
    public interface IInformationMessageService
    {
        Task AddAsync(InformationMessage informationMessage);

        Task<InformationMessage> GetAsync(Guid informationMessageId);

        Task<IEnumerable<InformationMessage>> GetAllAsync(Guid userId, DateTime updatedAfter);

        Task<IEnumerable<InformationMessage>> GetAllShallowAsync();

        Task UpdateAsync(InformationMessage informationMessage);

        Task<DateTime> GetLastUpdateAsync();
    }
}
