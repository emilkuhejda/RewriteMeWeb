using System;
using System.Threading.Tasks;
using RewriteMe.Domain.Administration;

namespace RewriteMe.Domain.Interfaces.Services
{
    public interface IAdministratorService
    {
        Task AddAsync(Administrator administrator);

        Task<Administrator> GetAsync(string username);

        Task<Administrator> GetAsync(Guid administratorId);
    }
}
