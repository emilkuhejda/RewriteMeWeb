using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using RewriteMe.Common.Helpers;
using RewriteMe.Common.Utils;
using RewriteMe.Domain.Enums;
using RewriteMe.Domain.Interfaces.Repositories;
using RewriteMe.Domain.Interfaces.Services;
using Serilog;

namespace RewriteMe.Business.Services
{
    public class CleanUpService : ICleanUpService
    {
        private readonly IFileAccessService _fileAccessService;
        private readonly IFileItemRepository _fileItemRepository;
        private readonly ILogger _logger;

        public CleanUpService(
            IFileAccessService fileAccessService,
            IFileItemRepository fileItemRepository,
            ILogger logger)
        {
            _fileAccessService = fileAccessService;
            _fileItemRepository = fileItemRepository;
            _logger = logger;
        }

        public void CleanUp(DateTime deleteBefore, CleanUpSettings cleanUpSettings, bool forceCleanUp)
        {
            AsyncHelper.RunSync(() => CleanUpAsync(deleteBefore, cleanUpSettings, forceCleanUp));
        }

        private async Task CleanUpAsync(DateTime deleteBefore, CleanUpSettings cleanUpSettings, bool forceCleanUp)
        {
            if (cleanUpSettings == CleanUpSettings.None)
                return;

            try
            {
                _logger.Information($"Start cleaning file item sources at '{DateTime.UtcNow}' with settings: border datetime='{deleteBefore}', settings='{cleanUpSettings.ToString()}', force cleanup = '{forceCleanUp}'.");

                var count = await CleanUpInternalAsync(deleteBefore, cleanUpSettings, forceCleanUp).ConfigureAwait(false);

                _logger.Information($"Clean up was successfully finished. {count} file items was processed.");
            }
            catch (Exception ex)
            {
                _logger.Fatal($"During cleanup error occurred.");
                _logger.Fatal(ExceptionFormatter.FormatException(ex));

                throw;
            }
        }

        private async Task<int> CleanUpInternalAsync(DateTime deleteBefore, CleanUpSettings cleanUpSettings, bool forceCleanUp)
        {
            var fileItemsEnumerable = await _fileItemRepository.GetFileItemsForCleaningAsync(deleteBefore, forceCleanUp).ConfigureAwait(false);
            var fileItems = fileItemsEnumerable.ToList();
            foreach (var fileItem in fileItems)
            {
                if (cleanUpSettings.HasFlag(CleanUpSettings.Disk))
                {
                    CleanDataFromDiskAsync(fileItem.UserId, fileItem.FileItemId);
                }

                if (cleanUpSettings.HasFlag(CleanUpSettings.Database))
                {
                    await _fileItemRepository.CleanSourceDataAsync(fileItem.FileItemId).ConfigureAwait(false);
                }

                await _fileItemRepository.MarkAsCleanedAsync(fileItem.FileItemId).ConfigureAwait(false);
            }

            return fileItems.Count;
        }

        private void CleanDataFromDiskAsync(Guid userId, Guid fileItemId)
        {
            var directoryPath = _fileAccessService.GetFileItemRootDirectory(userId, fileItemId);
            var directoryInfo = new DirectoryInfo(directoryPath);
            if (directoryInfo.Exists)
            {
                directoryInfo.Delete(true);
            }
        }
    }
}
