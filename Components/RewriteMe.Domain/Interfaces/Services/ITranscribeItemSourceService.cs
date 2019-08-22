using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RewriteMe.Domain.Transcription;

namespace RewriteMe.Domain.Interfaces.Services
{
    public interface ITranscribeItemSourceService
    {
        Task AddWavFileSources(Guid fileItemId, IEnumerable<WavPartialFile> wavFiles);
    }
}
