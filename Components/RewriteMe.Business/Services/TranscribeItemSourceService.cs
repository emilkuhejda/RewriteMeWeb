using System.Threading.Tasks;
using RewriteMe.Domain.Interfaces.Repositories;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.Domain.Transcription;

namespace RewriteMe.Business.Services
{
    public class TranscribeItemSourceService : ITranscribeItemSourceService
    {
        private readonly ITranscribeItemSourceRepository _transcribeItemSourceRepository;

        public TranscribeItemSourceService(ITranscribeItemSourceRepository transcribeItemSourceRepository)
        {
            _transcribeItemSourceRepository = transcribeItemSourceRepository;
        }

        public async Task AddAsync(TranscribeItemSource transcribeItemSource)
        {
            await _transcribeItemSourceRepository.AddAsync(transcribeItemSource).ConfigureAwait(false);
        }
    }
}
