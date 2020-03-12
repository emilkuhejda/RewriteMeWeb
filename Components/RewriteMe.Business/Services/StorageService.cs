using System.Linq;
using System.Threading.Tasks;
using RewriteMe.Domain.Interfaces.Repositories;
using RewriteMe.Domain.Interfaces.Services;

namespace RewriteMe.Business.Services
{
    public class StorageService : IStorageService
    {
        private readonly IFileItemRepository _fileItemRepository;

        public StorageService(IFileItemRepository fileItemRepository)
        {
            _fileItemRepository = fileItemRepository;
        }

        public async Task MigrateAsync()
        {
            var fileItemsToMigrate = (await _fileItemRepository.GetFileItemsForMigrationAsync().ConfigureAwait(false)).ToList();
            if (!fileItemsToMigrate.Any())
                return;
        }
    }
}
