using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RewriteMe.Domain.UserManagement;

namespace RewriteMe.Domain.Interfaces.Services
{
    public interface IDeletedUserService
    {
        Task<IEnumerable<DeletedUser>> GetAllAsync();

        Task DeleteAsync(Guid deletedUserId);
    }
}
