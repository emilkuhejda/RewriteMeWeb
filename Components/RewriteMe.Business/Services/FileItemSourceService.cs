using System;
using System.IO;
using System.Threading.Tasks;
using RewriteMe.Domain.Interfaces.Repositories;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.Domain.Transcription;

namespace RewriteMe.Business.Services
{
    public class FileItemSourceService : IFileItemSourceService
    {
        private readonly IFileAccessService _fileAccessService;
        private readonly IFileItemSourceRepository _fileItemSourceRepository;
        private readonly IApplicationLogService _applicationLogService;

        public FileItemSourceService(
            IFileAccessService fileAccessService,
            IFileItemSourceRepository fileItemSourceRepository,
            IApplicationLogService applicationLogService)
        {
            _fileAccessService = fileAccessService;
            _fileItemSourceRepository = fileItemSourceRepository;
            _applicationLogService = applicationLogService;
        }

        public async Task<FileItemSource> GetAsync(Guid fileItemId)
        {
            return await _fileItemSourceRepository.GetAsync(fileItemId).ConfigureAwait(false);
        }

        public async Task<bool> HasFileItemSourceAsync(Guid fileItemId)
        {
            return await _fileItemSourceRepository.HasFileItemSourceAsync(fileItemId).ConfigureAwait(false);
        }

        public async Task AddFileItemSourceAsync(FileItem fileItem)
        {
            var fileItemPath = _fileAccessService.GetOriginalFileItemPath(fileItem);
            if (!File.Exists(fileItemPath))
            {
                await _applicationLogService.ErrorAsync($"File '{fileItemPath}' was not found.").ConfigureAwait(false);
                return;
            }

            var source = await File.ReadAllBytesAsync(fileItemPath).ConfigureAwait(false);

            var fileItemSource = new FileItemSource
            {
                Id = Guid.NewGuid(),
                FileItemId = fileItem.Id,
                OriginalSource = source,
                DateCreated = DateTime.UtcNow
            };

            await _fileItemSourceRepository.AddAsync(fileItemSource).ConfigureAwait(false);
        }

        public async Task UpdateSource(Guid fileItemId, byte[] source)
        {
            await _fileItemSourceRepository.UpdateSource(fileItemId, source).ConfigureAwait(false);
        }
    }
}
