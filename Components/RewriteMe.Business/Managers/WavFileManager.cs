﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using RewriteMe.Common.Utils;
using RewriteMe.Domain.Enums;
using RewriteMe.Domain.Interfaces.Managers;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.Domain.Settings;
using RewriteMe.Domain.Transcription;
using Serilog;

namespace RewriteMe.Business.Managers
{
    public class WavFileManager : IWavFileManager
    {
        private readonly IFileItemService _fileItemService;
        private readonly IFileItemSourceService _fileItemSourceService;
        private readonly IWavFileService _wavFileService;
        private readonly ICacheService _cacheService;
        private readonly AppSettings _appSettings;
        private readonly ILogger _logger;

        public WavFileManager(
            IFileItemService fileItemService,
            IFileItemSourceService fileItemSourceService,
            IWavFileService wavFileService,
            ICacheService cacheService,
            IOptions<AppSettings> options,
            ILogger logger)
        {
            _fileItemService = fileItemService;
            _fileItemSourceService = fileItemSourceService;
            _wavFileService = wavFileService;
            _cacheService = cacheService;
            _appSettings = options.Value;
            _logger = logger.ForContext<WavFileManager>();
        }

        public async Task RunConversionToWavAsync(FileItem fileItem, Guid userId)
        {
            var convertedFileItemSourceExists = await _fileItemService.ConvertedFileItemSourceExistsAsync(fileItem).ConfigureAwait(false);
            if (convertedFileItemSourceExists)
                return;

            _logger.Information($"Start conversion to wav file. File item ID = {fileItem.Id}, File name = {fileItem.FileName}.");

            var directoryPath = string.Empty;
            try
            {
                await _fileItemService.UpdateRecognitionStateAsync(fileItem.Id, RecognitionState.Converting, _appSettings.ApplicationId).ConfigureAwait(false);
                await _cacheService.UpdateRecognitionStateAsync(fileItem.Id, RecognitionState.Converting).ConfigureAwait(false);

                directoryPath = _fileItemService.CreateUploadDirectoryIfNeeded(fileItem.UserId, fileItem.Id, fileItem.Storage == StorageSetting.Database);
                var filePath = await _fileItemService.GetOriginalFileItemPathAsync(fileItem, directoryPath).ConfigureAwait(false);
                if (string.IsNullOrWhiteSpace(filePath))
                    throw new InvalidOperationException(nameof(filePath));

                var sourceFile = await _wavFileService.ConvertToWavAsync(directoryPath, filePath).ConfigureAwait(false);
                var recognitionState = RecognitionState.Prepared;
                var source = await File.ReadAllBytesAsync(sourceFile.outputFilePath).ConfigureAwait(false);
                await _fileItemService.UpdateSourceFileNameAsync(fileItem.Id, sourceFile.fileName).ConfigureAwait(false);
                await _fileItemSourceService.UpdateSourceAsync(fileItem.Id, source).ConfigureAwait(false);
                await _fileItemService.UpdateRecognitionStateAsync(fileItem.Id, recognitionState, _appSettings.ApplicationId).ConfigureAwait(false);
                await _cacheService.UpdateRecognitionStateAsync(fileItem.Id, recognitionState).ConfigureAwait(false);

                fileItem.RecognitionState = recognitionState;
                fileItem.SourceFileName = sourceFile.fileName;

                var message = $"Conversion to wav file finished. File item ID = {fileItem.Id}, File name = {fileItem.FileName}, Old destination = {filePath}, New destination = {sourceFile.outputFilePath}.";
                _logger.Information(message);
            }
            catch (Exception ex)
            {
                _logger.Warning($"Exception occurred during conversion for file ID = '{fileItem.Id}'. [{fileItem.UserId}]");
                _logger.Warning(ExceptionFormatter.FormatException(ex));
            }
            finally
            {
                if (fileItem.Storage == StorageSetting.Database)
                {
                    _fileItemService.CleanUploadedData(directoryPath);
                }
            }
        }

        public async Task<IEnumerable<WavPartialFile>> SplitFileItemSourceAsync(FileItem fileItem, TimeSpan remainingTime)
        {
            var audioSource = await _fileItemService.GetAudioSourceAsync(fileItem).ConfigureAwait(false);
            return await _wavFileService.SplitWavFileAsync(audioSource, remainingTime, fileItem.Id, fileItem.UserId).ConfigureAwait(false);
        }
    }
}
