using System.Collections.Generic;
using System.Threading.Tasks;
using RewriteMe.Domain.Recording;

namespace RewriteMe.Domain.Interfaces.Repositories
{
    public interface ISpeechResultRepository
    {
        Task AddAsync(SpeechResult speechResult);

        Task UpdateAllAsync(IEnumerable<SpeechResult> speechResults);
    }
}
