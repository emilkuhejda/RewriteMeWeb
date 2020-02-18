using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using RewriteMe.Common.Helpers;
using RewriteMe.Common.Utils;
using RewriteMe.Domain.Enums;
using RewriteMe.Domain.Interfaces.Repositories;
using RewriteMe.Domain.Interfaces.Services;

namespace RewriteMe.Business.Services
{
    public class CleanUpService : ICleanUpService
    {
        private readonly IFileAccessService _fileAccessService;
        private readonly IApplicationLogService _applicationLogService;
        private readonly IFileItemRepository _fileItemRepository;

        public CleanUpService(
            IFileAccessService fileAccessService,
            IApplicationLogService applicationLogService,
            IFileItemRepository fileItemRepository)
        {
            _fileAccessService = fileAccessService;
            _applicationLogService = applicationLogService;
            _fileItemRepository = fileItemRepository;
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
                var message = $"Start cleaning file item sources at '{DateTime.UtcNow}' with settings: border datetime='{deleteBefore}', settings='{cleanUpSettings.ToString()}', force cleanup = '{forceCleanUp}'.";
                await _applicationLogService.InfoAsync(message).ConfigureAwait(false);

                var count = await CleanUpInternalAsync(deleteBefore, cleanUpSettings, forceCleanUp).ConfigureAwait(false);

                await _applicationLogService.InfoAsync($"Clean up was successfully finished. {count} file items was processed.").ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                await _applicationLogService.ErrorAsync($"During cleanup error occurred.{Environment.NewLine}{ExceptionFormatter.FormatException(ex)}").ConfigureAwait(false);
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
            var directoryPath = _fileAccessService.GetFileItemDirectory(userId, fileItemId);
            var directoryInfo = new DirectoryInfo(directoryPath);
            if (directoryInfo.Exists)
            {
                directoryInfo.Delete(true);
            }
        }
    }
}
