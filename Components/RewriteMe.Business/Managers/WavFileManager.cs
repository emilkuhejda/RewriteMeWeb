using System;
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

namespace RewriteMe.Business.Managers
{
    public class WavFileManager : IWavFileManager
    {
        private readonly IFileItemService _fileItemService;
        private readonly IFileItemSourceService _fileItemSourceService;
        private readonly IWavFileService _wavFileService;
        private readonly IApplicationLogService _applicationLogService;
        private readonly AppSettings _appSettings;

        public WavFileManager(
            IFileItemService fileItemService,
            IFileItemSourceService fileItemSourceService,
            IWavFileService wavFileService,
            IApplicationLogService applicationLogService,
            IOptions<AppSettings> options)
        {
            _fileItemService = fileItemService;
            _fileItemSourceService = fileItemSourceService;
            _wavFileService = wavFileService;
            _applicationLogService = applicationLogService;
            _appSettings = options.Value;
        }

        public async Task RunConversionToWavAsync(FileItem fileItem, Guid userId)
        {
            var convertedFileItemSourceExists = await _fileItemService.ConvertedFileItemSourceExistsAsync(fileItem).ConfigureAwait(false);
            if (convertedFileItemSourceExists)
                return;

            try
            {
                await _applicationLogService
                    .InfoAsync($"File WAV conversion is started for file ID: {fileItem.Id}.", userId)
                    .ConfigureAwait(false);

                await RunConversionToWavAsync(fileItem).ConfigureAwait(false);

                await _applicationLogService
                    .InfoAsync($"File WAV conversion is completed for file ID: {fileItem.Id}.", userId)
                    .ConfigureAwait(false);
            }
            catch
            {
                await _fileItemService.UpdateRecognitionStateAsync(fileItem.Id, RecognitionState.None, _appSettings.ApplicationId).ConfigureAwait(false);
                await _applicationLogService.InfoAsync($"File WAV conversion is not successful for file ID: {fileItem.Id}.", userId).ConfigureAwait(false);
                throw;
            }
        }

        private async Task RunConversionToWavAsync(FileItem fileItem)
        {
            await _fileItemService.UpdateRecognitionStateAsync(fileItem.Id, RecognitionState.Converting, _appSettings.ApplicationId).ConfigureAwait(false);

            var directoryPath = string.Empty;
            try
            {
                directoryPath = _fileItemService.CreateUploadDirectoryIfNeeded(fileItem.Id, fileItem.Storage == StorageSetting.Database);
                var filePath = await _fileItemService.GetOriginalFileItemPathAsync(fileItem, directoryPath).ConfigureAwait(false);
                if (string.IsNullOrWhiteSpace(filePath))
                    throw new InvalidOperationException(nameof(filePath));

                var sourceFile = await _wavFileService.ConvertToWavAsync(directoryPath, filePath).ConfigureAwait(false);
                var recognitionState = RecognitionState.Prepared;
                var source = await File.ReadAllBytesAsync(sourceFile.outputFilePath).ConfigureAwait(false);
                await _fileItemService.UpdateSourceFileNameAsync(fileItem.Id, sourceFile.fileName).ConfigureAwait(false);
                await _fileItemSourceService.UpdateSourceAsync(fileItem.Id, source).ConfigureAwait(false);
                await _fileItemService.UpdateRecognitionStateAsync(fileItem.Id, recognitionState, _appSettings.ApplicationId).ConfigureAwait(false);

                fileItem.RecognitionState = recognitionState;
                fileItem.SourceFileName = sourceFile.fileName;
            }
            catch (Exception ex)
            {
                var message = $"Exception occurred during conversion for file ID = '{fileItem.Id}'.{Environment.NewLine}{ExceptionFormatter.FormatException(ex)}";
                await _applicationLogService.ErrorAsync(message).ConfigureAwait(false);
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
            return await _wavFileService.SplitWavFileAsync(audioSource, remainingTime).ConfigureAwait(false);
        }
    }
}
