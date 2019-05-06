using System;
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
        private readonly IAudioSourceService _audioSourceService;
        private readonly IWavFileService _wavFileService;
        private readonly IApplicationLogService _applicationLogService;
        private readonly AppSettings _appSettings;

        public WavFileManager(
            IFileItemService fileItemService,
            IAudioSourceService audioSourceService,
            IWavFileService wavFileService,
            IApplicationLogService applicationLogService,
            IOptions<AppSettings> options)
        {
            _fileItemService = fileItemService;
            _audioSourceService = audioSourceService;
            _wavFileService = wavFileService;
            _applicationLogService = applicationLogService;
            _appSettings = options.Value;
        }

        public void RunConversionToWav(AudioSource audioSource, Guid userId)
        {
            try
            {
                _applicationLogService.InfoAsync($"File WAV conversion is started for file ID: {audioSource.FileItemId}.", userId);

                AsyncHelper.RunSync(() => RunConversionToWavAsync(audioSource));

                _applicationLogService.InfoAsync($"File WAV conversion is completed for file ID: {audioSource.FileItemId}.", userId);
            }
            catch
            {
                AsyncHelper.RunSync(() => _fileItemService.UpdateRecognitionStateAsync(audioSource.FileItemId, RecognitionState.None, _appSettings.ApplicationId));

                _applicationLogService.InfoAsync($"File WAV conversion is not successful for file ID: {audioSource.FileItemId}.", userId);
                throw;
            }
        }

        private async Task RunConversionToWavAsync(AudioSource audioSource)
        {
            await _fileItemService.UpdateRecognitionStateAsync(audioSource.FileItemId, RecognitionState.Converting, _appSettings.ApplicationId).ConfigureAwait(false);

            var wavFile = audioSource.IsWav()
                ? _wavFileService.CreateWavFileFromSource(audioSource.OriginalSource)
                : await _wavFileService.ConvertToWavAsync(audioSource.OriginalSource).ConfigureAwait(false);

            audioSource.WavSource = wavFile.Source;
            audioSource.TotalTime = wavFile.TotalTime;

            await _audioSourceService.UpdateAsync(audioSource).ConfigureAwait(false);
            await _fileItemService.UpdateRecognitionStateAsync(audioSource.FileItemId, RecognitionState.Prepared, _appSettings.ApplicationId).ConfigureAwait(false);
        }
    }
}
