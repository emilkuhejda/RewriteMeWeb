using System.Collections.Generic;
using System.Threading.Tasks;
using RewriteMe.Domain.Interfaces.Repositories;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.Domain.Recording;

namespace RewriteMe.Business.Services
{
    public class SpeechResultService : ISpeechResultService
    {
        private readonly ISpeechResultRepository _speechResultRepository;

        public SpeechResultService(ISpeechResultRepository speechResultRepository)
        {
            _speechResultRepository = speechResultRepository;
        }

        public async Task AddAsync(SpeechResult speechResult)
        {
            await _speechResultRepository.AddAsync(speechResult).ConfigureAwait(false);
        }

        public async Task UpdateAllAsync(IEnumerable<SpeechResult> speechResults)
        {
            await _speechResultRepository.UpdateAllAsync(speechResults).ConfigureAwait(false);
        }
    }
}
