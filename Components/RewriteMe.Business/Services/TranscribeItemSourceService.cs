using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using RewriteMe.Domain.Interfaces.Repositories;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.Domain.Transcription;
using Serilog;

namespace RewriteMe.Business.Services
{
    public class TranscribeItemSourceService : ITranscribeItemSourceService
    {
        private readonly ITranscribeItemSourceRepository _transcribeItemSourceRepository;
        private readonly ILogger _logger;

        public TranscribeItemSourceService(
            ITranscribeItemSourceRepository transcribeItemSourceRepository,
            ILogger logger)
        {
            _transcribeItemSourceRepository = transcribeItemSourceRepository;
            _logger = logger.ForContext<TranscribeItemSourceService>();
        }

        public async Task AddWavFileSourcesAsync(Guid fileItemId, IEnumerable<WavPartialFile> wavFiles)
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
                    DateCreatedUtc = DateTime.UtcNow
                };

                items.Add(transcribeItemSource);
            }

            if (!items.Any())
                return;

            await _transcribeItemSourceRepository.AddAsync(items).ConfigureAwait(false);

            _logger.Information($"Transcribe items for file item '{fileItemId}' were stored to database.");
        }
    }
}
