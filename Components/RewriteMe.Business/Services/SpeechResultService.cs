using System.Collections.Generic;
using System.Threading.Tasks;
using RewriteMe.Domain.Interfaces.Repositories;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.Domain.Recording;
using Serilog;

namespace RewriteMe.Business.Services
{
    public class SpeechResultService : ISpeechResultService
    {
        private readonly ISpeechResultRepository _speechResultRepository;
        private readonly ILogger _logger;

        public SpeechResultService(
            ISpeechResultRepository speechResultRepository,
            ILogger logger)
        {
            _speechResultRepository = speechResultRepository;
            _logger = logger.ForContext<SpeechResultService>();
        }

        public async Task AddAsync(SpeechResult speechResult)
        {
            await _speechResultRepository.AddAsync(speechResult).ConfigureAwait(false);

            _logger.Information($"Speech result '{speechResult.Id}' was created.");
        }

        public async Task UpdateAllAsync(IEnumerable<SpeechResult> speechResults)
        {
            await _speechResultRepository.UpdateAllAsync(speechResults).ConfigureAwait(false);

            _logger.Information("Speech results were updated.");
        }
    }
}
