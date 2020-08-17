using System.Linq;
using System.Threading.Tasks;
using RewriteMe.Domain.Interfaces.Managers;
using RewriteMe.Domain.Interfaces.Services;

namespace RewriteMe.Business.Services
{
    public class RestoreService : IRestoreService
    {
        private readonly IFileItemService _fileItemService;
        private readonly ISpeechRecognitionManager _speechRecognitionManager;

        private object _lockObject = new object();

        public RestoreService(IFileItemService fileItemService, ISpeechRecognitionManager speechRecognitionManager)
        {
            _fileItemService = fileItemService;
            _speechRecognitionManager = speechRecognitionManager;
        }

        private bool IsRunning { get; set; }

        public async Task RunAsync()
        {
            lock (_lockObject)
            {
                if (IsRunning)
                    return;

                IsRunning = true;
            }

            var fileItemsToRestore = (await _fileItemService.GetFileItemsInProgressAsync().ConfigureAwait(false)).ToList();
            if (!fileItemsToRestore.Any())
                return;

            var groupedFileItems = fileItemsToRestore.GroupBy(x => x.UserId);
            foreach (var group in groupedFileItems)
            {
                await Task.Run(async () =>
                {
                    foreach (var fileItemToRestore in group)
                    {
                        await _speechRecognitionManager.RunRecognitionAsync(fileItemToRestore.UserId, fileItemToRestore.Id, true).ConfigureAwait(false);
                    }
                }).ConfigureAwait(false);
            }

            IsRunning = false;
        }
    }
}
