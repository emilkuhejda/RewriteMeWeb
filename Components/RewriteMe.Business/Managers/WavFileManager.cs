using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using RewriteMe.Domain.Enums;
using RewriteMe.Domain.Extensions;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.Domain.Managers;
using RewriteMe.Domain.Settings;
using RewriteMe.Domain.Transcription;

namespace RewriteMe.Business.Managers
{
    public class WavFileManager : IWavFileManager
    {
        private readonly IFileItemService _fileItemService;
        private readonly IWavFileService _wavFileService;
        private readonly IFileAccessService _fileAccessService;
        private readonly IApplicationLogService _applicationLogService;
        private readonly AppSettings _appSettings;

        public WavFileManager(
            IFileItemService fileItemService,
            IWavFileService wavFileService,
            IFileAccessService fileAccessService,
            IApplicationLogService applicationLogService,
            IOptions<AppSettings> options)
        {
            _fileItemService = fileItemService;
            _wavFileService = wavFileService;
            _fileAccessService = fileAccessService;
            _applicationLogService = applicationLogService;
            _appSettings = options.Value;
        }

        public async Task RunConversionToWavAsync(FileItem fileItem, Guid userId)
        {
            if (fileItem.RecognitionState != RecognitionState.None && !string.IsNullOrWhiteSpace(fileItem.SourceFileName))
            {
                var convertedFilePath = _fileAccessService.GetFileItemPath(fileItem);
                if (File.Exists(convertedFilePath))
                    return;
            }

            var filePath = _fileAccessService.GetOriginalFileItemPath(fileItem);
            if (!File.Exists(filePath))
                return;

            try
            {
                await _applicationLogService.InfoAsync($"File WAV conversion is started for file ID: {fileItem.Id}.", userId).ConfigureAwait(false);

                await RunConversionToWavAsync(fileItem).ConfigureAwait(false);

                await _applicationLogService.InfoAsync($"File WAV conversion is completed for file ID: {fileItem.Id}.", userId).ConfigureAwait(false);
            }
            catch
            {
                await _fileItemService.UpdateRecognitionStateAsync(fileItem.Id, RecognitionState.None, _appSettings.ApplicationId).ConfigureAwait(false);
                await _applicationLogService.InfoAsync($"File WAV conversion is not successful for file ID: {fileItem.Id}.", userId);
                throw;
            }
        }

        private async Task RunConversionToWavAsync(FileItem fileItem)
        {
            await _fileItemService.UpdateRecognitionStateAsync(fileItem.Id, RecognitionState.Converting, _appSettings.ApplicationId).ConfigureAwait(false);

            string sourceFileName;
            if (!fileItem.IsOriginalWav())
            {
                sourceFileName = await _wavFileService.ConvertToWavAsync(fileItem).ConfigureAwait(false);
            }
            else
            {
                sourceFileName = _wavFileService.CopyWavAsync(fileItem);
            }

            var recognitionState = RecognitionState.Prepared;
            await _fileItemService.UpdateSourceFileNameAsync(fileItem.Id, sourceFileName).ConfigureAwait(false);
            await _fileItemService.UpdateRecognitionStateAsync(fileItem.Id, recognitionState, _appSettings.ApplicationId).ConfigureAwait(false);

            fileItem.RecognitionState = recognitionState;
            fileItem.SourceFileName = sourceFileName;
        }

        public async Task<IEnumerable<WavPartialFile>> SplitFileItemSourceAsync(Guid fileItemId, TimeSpan remainingTime)
        {
            var audioSource = await _fileItemService.GetAudioSource(fileItemId).ConfigureAwait(false);
            return await _wavFileService.SplitWavFileAsync(audioSource, remainingTime).ConfigureAwait(false);
        }
    }
}
