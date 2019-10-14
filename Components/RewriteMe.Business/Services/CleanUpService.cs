using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using RewriteMe.Domain.Enums;
using RewriteMe.Domain.Interfaces.Repositories;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.Domain.Settings;

namespace RewriteMe.Business.Services
{
    public class CleanUpService : ICleanUpService
    {
        private readonly IFileItemRepository _fileItemRepository;
        private readonly IFileAccessService _fileAccessService;
        private readonly AppSettings _appSettings;

        public CleanUpService(
            IFileItemRepository fileItemRepository,
            IFileAccessService fileAccessService,
            IOptions<AppSettings> options)
        {
            _fileItemRepository = fileItemRepository;
            _fileAccessService = fileAccessService;
            _appSettings = options.Value;
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

                await _fileItemRepository.UpdateDateAsync(fileItemId, _appSettings.ApplicationId).ConfigureAwait(false);
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
