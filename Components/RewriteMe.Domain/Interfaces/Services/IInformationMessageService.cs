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

        Task UpdateDatePublishedAsync(Guid informationMessageId, DateTime datePublished);

        Task<InformationMessage> MarkAsOpenedAsync(Guid userId, Guid informationMessageId);

        Task MarkAsOpenedAsync(Guid userId, IEnumerable<Guid> ids);

        Task<bool> CanUpdateAsync(Guid informationMessageId);

        Task<DateTime> GetLastUpdateAsync(Guid userId);
    }
}
