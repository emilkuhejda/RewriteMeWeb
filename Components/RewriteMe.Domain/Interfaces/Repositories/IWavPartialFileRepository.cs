using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RewriteMe.Domain.Transcription;

namespace RewriteMe.Domain.Interfaces.Repositories
{
    public interface IWavPartialFileRepository
    {
        Task AddAsync(IEnumerable<WavPartialFile> wavPartialFiles);

        Task<IEnumerable<WavPartialFile>> GetAsync(Guid fileItemId);
    }
}
