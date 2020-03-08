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
        private readonly IFileItemSourceRepository _fileItemSourceRepository;
        private readonly IApplicationLogService _applicationLogService;

        public FileItemSourceService(
            IFileItemSourceRepository fileItemSourceRepository,
            IApplicationLogService applicationLogService)
        {
            _fileItemSourceRepository = fileItemSourceRepository;
            _applicationLogService = applicationLogService;
        }

        public FileItemSource GetFileItemSource(Guid fileItemId)
        {
            return _fileItemSourceRepository.GetFileItemSource(fileItemId);
        }

        public async Task<bool> HasFileItemSourceAsync(Guid fileItemId)
        {
            return await _fileItemSourceRepository.HasFileItemSourceAsync(fileItemId).ConfigureAwait(false);
        }

        public async Task AddFileItemSourceAsync(FileItem fileItem, string fileItemPath)
        {
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
                DateCreatedUtc = DateTime.UtcNow
            };

            await _fileItemSourceRepository.AddAsync(fileItemSource).ConfigureAwait(false);
        }

        public async Task UpdateSourceAsync(Guid fileItemId, byte[] source)
        {
            await _fileItemSourceRepository.UpdateSourceAsync(fileItemId, source).ConfigureAwait(false);
        }
    }
}
