﻿using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using RewriteMe.Common.Helpers;
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
        private readonly IHostingEnvironmentService _hostingEnvironmentService;
        private readonly IApplicationLogService _applicationLogService;
        private readonly AppSettings _appSettings;

        public WavFileManager(
            IFileItemService fileItemService,
            IWavFileService wavFileService,
            IHostingEnvironmentService hostingEnvironmentService,
            IApplicationLogService applicationLogService,
            IOptions<AppSettings> options)
        {
            _fileItemService = fileItemService;
            _wavFileService = wavFileService;
            _hostingEnvironmentService = hostingEnvironmentService;
            _applicationLogService = applicationLogService;
            _appSettings = options.Value;
        }

        public void RunConversionToWav(FileItem fileItem, Guid userId)
        {
            var filePath = _hostingEnvironmentService.GetOriginalFileItemPath(fileItem);
            if (!File.Exists(filePath))
                return;

            try
            {
                _applicationLogService.InfoAsync($"File WAV conversion is started for file ID: {fileItem.Id}.", userId);

                AsyncHelper.RunSync(() => RunConversionToWavAsync(fileItem));

                _applicationLogService.InfoAsync($"File WAV conversion is completed for file ID: {fileItem.Id}.", userId);
            }
            catch
            {
                AsyncHelper.RunSync(() => _fileItemService.UpdateRecognitionStateAsync(fileItem.Id, RecognitionState.None, _appSettings.ApplicationId));

                _applicationLogService.InfoAsync($"File WAV conversion is not successful for file ID: {fileItem.Id}.", userId);
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

            await _fileItemService.UpdateSourceFileNameAsync(fileItem.Id, sourceFileName).ConfigureAwait(false);
            await _fileItemService.UpdateRecognitionStateAsync(fileItem.Id, RecognitionState.Prepared, _appSettings.ApplicationId).ConfigureAwait(false);
        }
    }
}
