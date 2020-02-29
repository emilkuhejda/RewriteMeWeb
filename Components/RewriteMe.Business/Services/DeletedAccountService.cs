using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RewriteMe.Domain.Interfaces.Repositories;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.Domain.UserManagement;

namespace RewriteMe.Business.Services
{
    public class DeletedAccountService : IDeletedAccountService
    {
        private readonly IDeletedAccountRepository _deletedAccountRepository;

        public DeletedAccountService(IDeletedAccountRepository deletedAccountRepository)
        {
            _deletedAccountRepository = deletedAccountRepository;
        }

        public async Task<IEnumerable<DeletedAccount>> GetAllAsync()
        {
            return await _deletedAccountRepository.GetAllAsync().ConfigureAwait(false);
        }

        public async Task DeleteAsync(Guid deletedAccountId)
        {
            await _deletedAccountRepository.DeleteAsync(deletedAccountId).ConfigureAwait(false);
        }
    }
}
