using System.Threading.Tasks;
using RewriteMe.DataAccess.DataAdapters;
using RewriteMe.Domain.Interfaces.Repositories;
using RewriteMe.Domain.Transcription;

namespace RewriteMe.DataAccess.Repositories
{
    public class UploadedChunkRepository : IUploadedChunkRepository
    {
        private readonly IDbContextFactory _contextFactory;

        public UploadedChunkRepository(IDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task AddAsync(UploadedChunk uploadedChunk)
        {
            using (var context = _contextFactory.Create())
            {
                await context.UploadedChunks.AddAsync(uploadedChunk.ToUploadedChunkEntity()).ConfigureAwait(false);
                await context.SaveChangesAsync().ConfigureAwait(false);
            }
        }
    }
}
