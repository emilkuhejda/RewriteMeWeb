using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RewriteMe.Domain.Administration;

namespace RewriteMe.Domain.Interfaces.Repositories
{
    public interface IAdministratorRepository
    {
        Task AddAsync(Administrator administrator);

        Task UpdateAsync(Administrator administrator);

        Task DeleteAsync(Guid administratorId);

        Task<IEnumerable<Administrator>> GetAllAsync();

        Task<Administrator> GetAsync(string username);

        Task<Administrator> GetAsync(Guid administratorId);
    }
}
