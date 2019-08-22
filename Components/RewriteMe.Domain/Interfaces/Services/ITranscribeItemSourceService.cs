using System.Threading.Tasks;
using RewriteMe.Domain.Transcription;

namespace RewriteMe.Domain.Interfaces.Services
{
    public interface ITranscribeItemSourceService
    {
        Task AddAsync(TranscribeItemSource transcribeItemSource);
    }
}
