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
        private readonly ICleanUpRepository _cleanUpRepository;
        private readonly IFileAccessService _fileAccessService;

        public CleanUpService(
            ICleanUpRepository cleanUpRepository,
            IFileAccessService fileAccessService)
        {
            _cleanUpRepository = cleanUpRepository;
            _fileAccessService = fileAccessService;
        }

        public async Task CleanAsync(DateTime deleteBefore, CleanUpSettings cleanUpSettings)
        {
            if (cleanUpSettings.HasFlag(CleanUpSettings.Disk))
            {
                await CleanDataFromDiskAsync(deleteBefore).ConfigureAwait(false);
            }

            if (cleanUpSettings.HasFlag(CleanUpSettings.Database))
            {
                await CleanDataFromDatabaseAsync(deleteBefore).ConfigureAwait(false);
            }
        }

        private async Task CleanDataFromDiskAsync(DateTime deleteBefore)
        {
            var fileItemIds = await _cleanUpRepository.GetFileItemIdsForCleaningAsync(deleteBefore).ConfigureAwait(false);
            foreach (var fileItemId in fileItemIds)
            {
                var directoryPath = _fileAccessService.GetFileItemDirectory(fileItemId);
                var directoryInfo = new DirectoryInfo(directoryPath);
                if (directoryInfo.Exists)
                {
                    directoryInfo.Delete(true);
                }
            }
        }

        private async Task CleanDataFromDatabaseAsync(DateTime deleteBefore)
        {
            await _cleanUpRepository.CleanFileItemSourcesAsync(deleteBefore).ConfigureAwait(false);
            await _cleanUpRepository.CleanTranscribeItemSourceAsync(deleteBefore).ConfigureAwait(false);
        }
    }
}
