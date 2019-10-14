using System;
using System.IO;
using System.Threading.Tasks;
using RewriteMe.Domain.Enums;
using RewriteMe.Domain.Interfaces.Repositories;
using RewriteMe.Domain.Interfaces.Services;

namespace RewriteMe.Business.Services
{
    public class CleanUpService : ICleanUpService
    {
        private readonly IFileItemRepository _fileItemRepository;
        private readonly IFileAccessService _fileAccessService;

        public CleanUpService(
            IFileItemRepository fileItemRepository,
            IFileAccessService fileAccessService)
        {
            _fileItemRepository = fileItemRepository;
            _fileAccessService = fileAccessService;
        }

        public async Task CleanAsync(DateTime deleteBefore, CleanUpSettings cleanUpSettings)
        {
            var fileItemIds = await _fileItemRepository.GetFileItemIdsForCleaningAsync(deleteBefore).ConfigureAwait(false);
            foreach (var fileItemId in fileItemIds)
            {
                if (cleanUpSettings.HasFlag(CleanUpSettings.Disk))
                {
                    CleanDataFromDiskAsync(fileItemId);
                }

                if (cleanUpSettings.HasFlag(CleanUpSettings.Database))
                {
                    await _fileItemRepository.CleanSourceDataAsync(fileItemId).ConfigureAwait(false);
                }

                await _fileItemRepository.MarkAsCleanedAsync(fileItemId).ConfigureAwait(false);
            }
        }

        private void CleanDataFromDiskAsync(Guid fileItemId)
        {
            var directoryPath = _fileAccessService.GetFileItemDirectory(fileItemId);
            var directoryInfo = new DirectoryInfo(directoryPath);
            if (directoryInfo.Exists)
            {
                directoryInfo.Delete(true);
            }
        }
    }
}
