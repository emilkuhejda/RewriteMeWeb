using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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

        public async Task<IEnumerable<UploadedChunk>> GetAllAsync(Guid fileItemId, Guid applicationId, CancellationToken cancellationToken)
        {
            using (var context = _contextFactory.Create())
            {
                var entities = await context.UploadedChunks
                    .Where(x => x.FileItemId == fileItemId && x.ApplicationId == applicationId)
                    .ToListAsync(cancellationToken)
                    .ConfigureAwait(false);

                return entities?.Select(x => x.ToUploadedChunk());
            }
        }

        public async Task DeleteAsync(Guid fileItemId, Guid applicationId)
        {
            using (var context = _contextFactory.Create())
            {
                var entities = await context.UploadedChunks
                    .Where(x => x.FileItemId == fileItemId && x.ApplicationId == applicationId)
                    .ToListAsync()
                    .ConfigureAwait(false);

                context.RemoveRange(entities);
                await context.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        public async Task CleanOutdatedChunksAsync(DateTime dateToCompare)
        {
            using (var context = _contextFactory.Create())
            {
                var entities = await context.UploadedChunks
                    .Where(x => x.DateCreatedUtc < dateToCompare)
                    .ToListAsync()
                    .ConfigureAwait(false);

                if (!entities.Any())
                    return;

                context.RemoveRange(entities);
                await context.SaveChangesAsync().ConfigureAwait(false);
            }
        }
    }
}
