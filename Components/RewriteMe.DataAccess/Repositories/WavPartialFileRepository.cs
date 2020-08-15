using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RewriteMe.DataAccess.DataAdapters;
using RewriteMe.DataAccess.Entities;
using RewriteMe.Domain.Interfaces.Repositories;
using RewriteMe.Domain.Transcription;

namespace RewriteMe.DataAccess.Repositories
{
    public class WavPartialFileRepository : IWavPartialFileRepository
    {
        private readonly IDbContextFactory _contextFactory;

        public WavPartialFileRepository(IDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task AddAsync(WavPartialFile wavPartialFile)
        {
            using (var context = _contextFactory.Create())
            {
                await context.AddAsync(wavPartialFile.ToWavPartialFileEntity()).ConfigureAwait(false);
                await context.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        public async Task<IEnumerable<WavPartialFile>> GetAsync(Guid fileItemId)
        {
            using (var context = _contextFactory.Create())
            {
                var entities = await context.WavPartialFiles.Where(x => x.FileItemId == fileItemId)
                    .ToListAsync()
                    .ConfigureAwait(false);

                return entities.Select(x => x.ToWavPartialFile());
            }
        }

        public async Task DeleteAsync(Guid partialFileId)
        {
            using (var context = _contextFactory.Create())
            {
                var partialFileEntity = new WavPartialFileEntity { Id = partialFileId };
                context.Entry(partialFileEntity).State = EntityState.Deleted;
                await context.SaveChangesAsync().ConfigureAwait(false);
            }
        }
    }
}
