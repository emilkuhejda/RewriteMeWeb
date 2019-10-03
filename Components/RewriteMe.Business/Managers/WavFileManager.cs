using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using RewriteMe.Business.Configuration;
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
        private readonly IFileItemSourceService _fileItemSourceService;
        private readonly IInternalValueService _internalValueService;
        private readonly IWavFileService _wavFileService;
        private readonly IFileAccessService _fileAccessService;
        private readonly IApplicationLogService _applicationLogService;
        private readonly AppSettings _appSettings;

        public WavFileManager(
            IFileItemService fileItemService,
            IFileItemSourceService fileItemSourceService,
            IInternalValueService internalValueService,
            IWavFileService wavFileService,
            IFileAccessService fileAccessService,
            IApplicationLogService applicationLogService,
            IOptions<AppSettings> options)
        {
            _fileItemService = fileItemService;
            _fileItemSourceService = fileItemSourceService;
            _internalValueService = internalValueService;
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

            string filePath;
            var readSourceFromDatabase = await _internalValueService.GetValueAsync(InternalValues.ReadSourceFromDatabase).ConfigureAwait(false);
            if (readSourceFromDatabase)
            {
                var fileItemSource = await _fileItemSourceService.GetAsync(fileItem.Id).ConfigureAwait(false);
                if (fileItemSource.OriginalSource == null || !fileItemSource.OriginalSource.Any())
                    return;

                filePath = GetTempFullPath();
                await File.WriteAllBytesAsync(filePath, fileItemSource.OriginalSource).ConfigureAwait(false);
            }
            else
            {
                filePath = _fileAccessService.GetOriginalFileItemPath(fileItem);
                if (!File.Exists(filePath))
                    return;
            }

            try
            {
                await _applicationLogService
                    .InfoAsync($"File WAV conversion is started for file ID: {fileItem.Id}.", userId)
                    .ConfigureAwait(false);

                await RunConversionToWavAsync(fileItem, filePath).ConfigureAwait(false);

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
            finally
            {
                if (readSourceFromDatabase)
                {
                    File.Delete(filePath);
                }
            }
        }

        private string GetTempFullPath()
        {
            return Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.wav");
        }

        private async Task RunConversionToWavAsync(FileItem fileItem, string inputFilePath)
        {
            await _fileItemService.UpdateRecognitionStateAsync(fileItem.Id, RecognitionState.Converting, _appSettings.ApplicationId).ConfigureAwait(false);

            (string outputFilePath, string fileName) sourceFile;
            if (!fileItem.IsOriginalWav())
            {
                sourceFile = await _wavFileService.ConvertToWavAsync(fileItem, inputFilePath).ConfigureAwait(false);
            }
            else
            {
                sourceFile = _wavFileService.CopyWavAsync(fileItem, inputFilePath);
            }

            var recognitionState = RecognitionState.Prepared;
            var source = await File.ReadAllBytesAsync(sourceFile.outputFilePath).ConfigureAwait(false);
            await _fileItemService.UpdateSourceFileNameAsync(fileItem.Id, sourceFile.fileName).ConfigureAwait(false);
            await _fileItemSourceService.UpdateSource(fileItem.Id, source).ConfigureAwait(false);
            await _fileItemService.UpdateRecognitionStateAsync(fileItem.Id, recognitionState, _appSettings.ApplicationId).ConfigureAwait(false);

            fileItem.RecognitionState = recognitionState;
            fileItem.SourceFileName = sourceFile.fileName;
        }

        public async Task<IEnumerable<WavPartialFile>> SplitFileItemSourceAsync(Guid fileItemId, TimeSpan remainingTime)
        {
            var audioSource = await _fileItemService.GetAudioSource(fileItemId).ConfigureAwait(false);
            return await _wavFileService.SplitWavFileAsync(audioSource, remainingTime).ConfigureAwait(false);
        }
    }
}
