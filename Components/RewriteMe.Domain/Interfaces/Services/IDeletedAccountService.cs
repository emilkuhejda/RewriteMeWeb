using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RewriteMe.Domain.UserManagement;

namespace RewriteMe.Domain.Interfaces.Services
{
    public interface IDeletedAccountService
    {
        Task<IEnumerable<DeletedAccount>> GetAllAsync();

        Task DeleteAsync(Guid deletedAccountId);
    }
}
