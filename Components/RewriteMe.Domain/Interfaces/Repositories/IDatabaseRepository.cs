using System.Threading.Tasks;

namespace RewriteMe.Domain.Interfaces.Repositories
{
    public interface IDatabaseRepository
    {
        Task ResetAsync();

        Task DeleteDatabaseAsync();
    }
}
