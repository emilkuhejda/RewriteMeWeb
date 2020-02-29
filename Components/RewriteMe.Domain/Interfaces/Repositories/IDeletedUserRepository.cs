using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RewriteMe.Domain.UserManagement;

namespace RewriteMe.Domain.Interfaces.Repositories
{
    public interface IDeletedUserRepository
    {
        Task<IEnumerable<DeletedUser>> GetAllAsync();

        Task AddAsync(DeletedUser deletedUser);

        Task DeleteAsync(Guid deletedUserId);
    }
}
