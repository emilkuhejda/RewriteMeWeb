using System;
using System.IO;
using System.Threading.Tasks;
using RewriteMe.Domain.Interfaces.Repositories;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.Domain.Transcription;
using Serilog;

namespace RewriteMe.Business.Services
{
    public class FileItemSourceService : IFileItemSourceService
    {
        private readonly IFileItemSourceRepository _fileItemSourceRepository;
        private readonly ILogger _logger;

        public FileItemSourceService(
            IFileItemSourceRepository fileItemSourceRepository,
            ILogger logger)
        {
            _fileItemSourceRepository = fileItemSourceRepository;
            _logger = logger.ForContext<FileItemSourceService>();
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
                _logger.Warning($"File '{fileItemPath}' was not found.");

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

            _logger.Information($"File item source '{fileItemSource.Id}' for file item '{fileItem.Id}' was created.");
        }

        public async Task UpdateSourceAsync(Guid fileItemId, byte[] source)
        {
            await _fileItemSourceRepository.UpdateSourceAsync(fileItemId, source).ConfigureAwait(false);

            _logger.Information($"File item '{fileItemId}' source was updated.");
        }
    }
}
