using System.Threading.Tasks;

namespace RewriteMe.Domain.Interfaces.Services
{
    public interface IStorageService
    {
        Task MigrateAsync();
    }
}
