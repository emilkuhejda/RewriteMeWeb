using System;
using System.Threading.Tasks;
using RewriteMe.Common.Helpers;
using RewriteMe.Domain.Enums;
using RewriteMe.Domain.Extensions;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.Domain.Managers;
using RewriteMe.Domain.Transcription;

namespace RewriteMe.Business.Managers
{
    public class WavFileManager : IWavFileManager
    {
        private readonly IFileItemService _fileItemService;
        private readonly IAudioSourceService _audioSourceService;
        private readonly IWavFileService _wavFileService;
        private readonly IApplicationLogService _applicationLogService;

        public WavFileManager(
            IFileItemService fileItemService,
            IAudioSourceService audioSourceService,
            IWavFileService wavFileService,
            IApplicationLogService applicationLogService)
        {
            _fileItemService = fileItemService;
            _audioSourceService = audioSourceService;
            _wavFileService = wavFileService;
            _applicationLogService = applicationLogService;
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
                _applicationLogService.InfoAsync($"File WAV conversion is not successful for file ID: {audioSource.FileItemId}.", userId);
                throw;
            }
        }

        private async Task RunConversionToWavAsync(AudioSource audioSource)
        {
            if (!audioSource.IsSupportedType())
                throw new InvalidOperationException("File type is not supported");

            await _fileItemService.UpdateRecognitionStateAsync(audioSource.FileItemId, RecognitionState.Converting).ConfigureAwait(false);

            var wavFile = audioSource.IsWav()
                ? _wavFileService.CreateWavFileFromSource(audioSource.OriginalSource)
                : await _wavFileService.ConvertToWavAsync(audioSource.OriginalSource).ConfigureAwait(false);

            audioSource.WavSource = wavFile.Source;
            audioSource.TotalTime = wavFile.TotalTime;

            await _audioSourceService.UpdateAsync(audioSource).ConfigureAwait(false);
            await _fileItemService.UpdateRecognitionStateAsync(audioSource.FileItemId, RecognitionState.Prepared);
        }
    }
}
