using System.Threading.Tasks;
using RewriteMe.Domain.Logging;

namespace RewriteMe.Domain.Interfaces.Services
{
    public interface IApplicationLogService
    {
        Task Add(ApplicationLog applicationLog);
    }
}
