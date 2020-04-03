using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RewriteMe.Domain.Interfaces.Repositories;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.Domain.UserManagement;
using Serilog;

namespace RewriteMe.Business.Services
{
    public class DeletedAccountService : IDeletedAccountService
    {
        private readonly IDeletedAccountRepository _deletedAccountRepository;
        private readonly ILogger _logger;

        public DeletedAccountService(
            IDeletedAccountRepository deletedAccountRepository,
            ILogger logger)
        {
            _deletedAccountRepository = deletedAccountRepository;
            _logger = logger.ForContext<DeletedAccountService>();
        }

        public async Task<IEnumerable<DeletedAccount>> GetAllAsync()
        {
            return await _deletedAccountRepository.GetAllAsync().ConfigureAwait(false);
        }

        public async Task AddAsync(DeletedAccount deletedAccount)
        {
            await _deletedAccountRepository.AddAsync(deletedAccount).ConfigureAwait(false);

            _logger.Information($"Account for erase was created. User ID = '{deletedAccount.UserId}'.");
        }

        public async Task DeleteAsync(Guid deletedAccountId)
        {
            await _deletedAccountRepository.DeleteAsync(deletedAccountId).ConfigureAwait(false);

            _logger.Information($"Deleted account was removed. User ID = '{deletedAccountId}'.");
        }
    }
}
