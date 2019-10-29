using System.Threading.Tasks;

namespace RewriteMe.Domain.Interfaces.Services
{
    public interface IDatabaseService
    {
        Task ResetAsync();

        Task DeleteDatabaseAsync();
    }
}
