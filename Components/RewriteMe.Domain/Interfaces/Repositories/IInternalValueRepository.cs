using System.Threading.Tasks;

namespace RewriteMe.Domain.Interfaces.Repositories
{
    public interface IInternalValueRepository
    {
        Task<string> GetValueAsync(string key);

        Task UpdateValueAsync(string key, string value);
    }
}
