using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RewriteMe.Domain.Interfaces.Repositories;
using RewriteMe.Domain.Transcription;

namespace RewriteMe.DataAccess.Repositories
{
    public class WavPartialFileRepository : IWavPartialFileRepository
    {
        public Task AddAsync(IEnumerable<WavPartialFile> wavPartialFiles)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<WavPartialFile>> GetAsync(Guid fileItemId)
        {
            throw new NotImplementedException();
        }
    }
}
