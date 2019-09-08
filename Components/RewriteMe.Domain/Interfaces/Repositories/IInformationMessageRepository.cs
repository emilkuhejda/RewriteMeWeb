using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RewriteMe.Domain.Messages;

namespace RewriteMe.Domain.Interfaces.Repositories
{
    public interface IInformationMessageRepository
    {
        Task AddAsync(InformationMessage informationMessage);

        Task<InformationMessage> GetAsync(Guid informationMessageId);

        Task<IEnumerable<InformationMessage>> GetAllAsync(Guid userId, DateTime updatedAfter);

        Task<IEnumerable<InformationMessage>> GetAllShallowAsync();

        Task UpdateAsync(InformationMessage informationMessage);

        Task UpdateDatePublishedAsync(Guid informationMessageId, DateTime datePublished);

        Task<InformationMessage> MarkAsOpened(Guid userId, Guid informationMessageId);

        Task<bool> CanUpdateAsync(Guid informationMessageId);

        Task<DateTime> GetLastUpdateAsync();
    }
}
