using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using RewriteMe.Domain.Interfaces.Repositories;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.Domain.Transcription;

namespace RewriteMe.Business.Services
{
    public class WavPartialFileService : IWavPartialFileService
    {
        private readonly IWavPartialFileRepository _wavPartialFileRepository;

        public WavPartialFileService(IWavPartialFileRepository wavPartialFileRepository)
        {
            _wavPartialFileRepository = wavPartialFileRepository;
        }

        public async Task AddAsync(WavPartialFile wavPartialFile)
        {
            await _wavPartialFileRepository.AddAsync(wavPartialFile).ConfigureAwait(false);
        }

        public async Task<IEnumerable<WavPartialFile>> GetAsync(Guid fileItemId)
        {
            return await _wavPartialFileRepository.GetAsync(fileItemId).ConfigureAwait(false);
        }

        public async Task DeleteAsync(WavPartialFile partialFile)
        {
            if (File.Exists(partialFile.Path))
                File.Delete(partialFile.Path);

            await _wavPartialFileRepository.DeleteAsync(partialFile.Id).ConfigureAwait(false);
        }
    }
}
