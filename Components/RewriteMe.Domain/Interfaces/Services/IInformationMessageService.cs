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

        Task<IEnumerable<InformationMessage>> GetAllAsync(Guid userId, DateTime updatedAfter, int? count);

        Task<IEnumerable<InformationMessage>> GetAllShallowAsync();

        Task UpdateAsync(InformationMessage informationMessage);

        Task UpdateDatePublishedAsync(Guid informationMessageId, DateTime datePublished);

        Task<bool> CanUpdateAsync(Guid informationMessageId);

        Task<DateTime> GetLastUpdateAsync();
    }
}
