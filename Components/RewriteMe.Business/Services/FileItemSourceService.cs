using System.Threading.Tasks;
using RewriteMe.Domain.Interfaces.Repositories;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.Domain.Transcription;

namespace RewriteMe.Business.Services
{
    public class FileItemSourceService : IFileItemSourceService
    {
        private readonly IFileItemSourceRepository _fileItemSourceRepository;

        public FileItemSourceService(IFileItemSourceRepository fileItemSourceRepository)
        {
            _fileItemSourceRepository = fileItemSourceRepository;
        }

        public async Task AddAsync(FileItemSource fileItemSource)
        {
            await _fileItemSourceRepository.AddAsync(fileItemSource).ConfigureAwait(false);
        }
    }
}
