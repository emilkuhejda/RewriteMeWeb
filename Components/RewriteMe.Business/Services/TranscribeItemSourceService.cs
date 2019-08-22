using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using RewriteMe.Domain.Interfaces.Repositories;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.Domain.Transcription;

namespace RewriteMe.Business.Services
{
    public class TranscribeItemSourceService : ITranscribeItemSourceService
    {
        private readonly ITranscribeItemSourceRepository _transcribeItemSourceRepository;

        public TranscribeItemSourceService(ITranscribeItemSourceRepository transcribeItemSourceRepository)
        {
            _transcribeItemSourceRepository = transcribeItemSourceRepository;
        }

        public async Task AddWavFileSources(Guid fileItemId, IEnumerable<WavPartialFile> wavFiles)
        {
            var items = new List<TranscribeItemSource>();
            foreach (var wavFile in wavFiles)
            {
                if (!File.Exists(wavFile.Path))
                    continue;

                var source = await File.ReadAllBytesAsync(wavFile.Path).ConfigureAwait(false);
                var transcribeItemSource = new TranscribeItemSource
                {
                    Id = wavFile.Id,
                    FileItemId = fileItemId,
                    Source = source,
                    DateCreated = DateTime.UtcNow
                };

                items.Add(transcribeItemSource);
            }

            if (!items.Any())
                return;

            await _transcribeItemSourceRepository.AddAsync(items).ConfigureAwait(false);
        }
    }
}
