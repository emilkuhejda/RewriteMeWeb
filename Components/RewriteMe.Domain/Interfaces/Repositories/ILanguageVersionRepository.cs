using System.Threading.Tasks;
using RewriteMe.Domain.Messages;

namespace RewriteMe.Domain.Interfaces.Repositories
{
    public interface ILanguageVersionRepository
    {
        Task UpdateAsync(LanguageVersion languageVersion);
    }
}
