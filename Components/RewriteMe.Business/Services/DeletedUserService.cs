using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RewriteMe.Domain.Interfaces.Repositories;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.Domain.UserManagement;

namespace RewriteMe.Business.Services
{
    public class DeletedUserService : IDeletedUserService
    {
        private readonly IDeletedUserRepository _deletedUserRepository;

        public DeletedUserService(IDeletedUserRepository deletedUserRepository)
        {
            _deletedUserRepository = deletedUserRepository;
        }

        public async Task<IEnumerable<DeletedUser>> GetAllAsync()
        {
            return await _deletedUserRepository.GetAllAsync().ConfigureAwait(false);
        }

        public async Task DeleteAsync(Guid deletedUserId)
        {
            await _deletedUserRepository.DeleteAsync(deletedUserId).ConfigureAwait(false);
        }
    }
}
