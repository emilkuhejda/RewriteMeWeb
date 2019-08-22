using System.Threading.Tasks;
using RewriteMe.DataAccess.DataAdapters;
using RewriteMe.Domain.Interfaces.Repositories;
using RewriteMe.Domain.Transcription;

namespace RewriteMe.DataAccess.Repositories
{
    public class FileItemSourceRepository : IFileItemSourceRepository
    {
        private readonly IDbContextFactory _contextFactory;

        public FileItemSourceRepository(IDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task AddAsync(FileItemSource fileItemSource)
        {
            using (var context = _contextFactory.Create())
            {
                await context.FileItemSources.AddAsync(fileItemSource.ToFileItemSourceEntity()).ConfigureAwait(false);
                await context.SaveChangesAsync().ConfigureAwait(false);
            }
        }
    }
}
