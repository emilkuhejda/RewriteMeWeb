using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RewriteMe.Domain.Transcription;

namespace RewriteMe.Domain.Interfaces.Services
{
    public interface IWavPartialFileService
    {
        Task AddAsync(WavPartialFile wavPartialFile);

        Task<IEnumerable<WavPartialFile>> GetAsync(Guid fileItemId);

        Task DeleteAsync(WavPartialFile partialFile);
    }
}
