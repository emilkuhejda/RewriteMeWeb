using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.Domain.Transcription;

namespace RewriteMe.Business.Services
{
    public class WavPartialFileService : IWavPartialFileService
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
