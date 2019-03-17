using System.Threading.Tasks;
using RewriteMe.Domain.Logging;

namespace RewriteMe.Domain.Interfaces.Repositories
{
    public interface IApplicationLogRepository
    {
        Task AddAsync(ApplicationLog applicationLog);
    }
}
