using System.Threading.Tasks;
using RewriteMe.Domain.Administration;

namespace RewriteMe.Domain.Interfaces.Repositories
{
    public interface IAdministratorRepository
    {
        Task AddAsync(Administrator administrator);

        Task<Administrator> GetAsync(string username);
    }
}
