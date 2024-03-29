﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using RewriteMe.Domain.Interfaces.Repositories;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.Domain.Transcription;
using Serilog;

namespace RewriteMe.Business.Services
{
    public class WavPartialFileService : IWavPartialFileService
    {
        private readonly IWavPartialFileRepository _wavPartialFileRepository;
        private readonly IFileAccessService _fileAccessService;
        private readonly ILogger _logger;

        public WavPartialFileService(
            IWavPartialFileRepository wavPartialFileRepository,
            IFileAccessService fileAccessService,
            ILogger logger)
        {
            _wavPartialFileRepository = wavPartialFileRepository;
            _fileAccessService = fileAccessService;
            _logger = logger.ForContext<WavPartialFileService>();
        }

        public async Task AddAsync(WavPartialFile wavPartialFile)
        {
            await _wavPartialFileRepository.AddAsync(wavPartialFile).ConfigureAwait(false);

            _logger.Information($"Wav partial file '{wavPartialFile.Path}' was created.");
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

            _logger.Information($"Wav partial file '{partialFile.Path}' was deleted.");
        }

        public void DeleteDirectory(Guid userId, Guid fileItemId)
        {
            var directoryPath = _fileAccessService.GetPartialFilesDirectoryPath(userId, fileItemId);
            Directory.Delete(directoryPath, true);

            _logger.Information("Delete directory for partial files.");
        }
    }
}
