using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using RewriteMe.Common.Helpers;
using RewriteMe.Domain.Enums;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.Domain.Managers;
using RewriteMe.Domain.Settings;
using RewriteMe.Domain.Transcription;

namespace RewriteMe.Business.Managers
{
    public class SpeechRecognitionManager : ISpeechRecognitionManager
    {
        private readonly ISpeechRecognitionService _speechRecognitionService;
        private readonly IFileItemService _fileItemService;
        private readonly ITranscribeItemService _transcribeItemService;
        private readonly IWavFileService _wavFileService;
        private readonly IUserSubscriptionService _userSubscriptionService;
        private readonly IApplicationLogService _applicationLogService;
        private readonly IWavFileManager _wavFileManager;
        private readonly AppSettings _appSettings;

        public SpeechRecognitionManager(
            ISpeechRecognitionService speechRecognitionService,
            IFileItemService fileItemService,
            ITranscribeItemService transcribeItemService,
            IWavFileService wavFileService,
            IUserSubscriptionService userSubscriptionService,
            IApplicationLogService applicationLogService,
            IWavFileManager wavFileManager,
            IOptions<AppSettings> options)
        {
            _speechRecognitionService = speechRecognitionService;
            _fileItemService = fileItemService;
            _transcribeItemService = transcribeItemService;
            _wavFileService = wavFileService;
            _userSubscriptionService = userSubscriptionService;
            _applicationLogService = applicationLogService;
            _wavFileManager = wavFileManager;
            _appSettings = options.Value;
        }

        public async Task<bool> CanRunRecognition(Guid userId)
        {
            var subscriptionRemainingTime = await _userSubscriptionService.GetRemainingTime(userId).ConfigureAwait(false);
            return subscriptionRemainingTime.TotalSeconds > 15;
        }

        public void RunRecognition(Guid userId, Guid fileItemId)
        {
            AsyncHelper.RunSync(() => RunRecognitionAsync(userId, fileItemId));
        }

        private async Task RunRecognitionAsync(Guid userId, Guid fileItemId)
        {
            var fileItem = await _fileItemService.GetAsync(userId, fileItemId).ConfigureAwait(false);
            await _wavFileManager.RunConversionToWavAsync(fileItem, userId).ConfigureAwait(false);

            await _applicationLogService.InfoAsync($"Attempt to start Speech recognition for file ID: '{fileItem.Id}'.", userId).ConfigureAwait(false);
            if (fileItem.RecognitionState < RecognitionState.Prepared)
            {
                var message = $"File with ID: '{fileItem.Id}' is stil converting. Speech recognition is stopped.";
                await _applicationLogService.ErrorAsync(message, userId).ConfigureAwait(false);
                throw new InvalidOperationException(message);
            }

            var canRunRecognition = await CanRunRecognition(fileItem.UserId).ConfigureAwait(false);
            if (!canRunRecognition)
            {
                var message = $"User ID = '{fileItem.UserId}' does not have enough free minutes in the subscription.";
                await _applicationLogService.ErrorAsync(message, fileItem.UserId).ConfigureAwait(false);
                throw new InvalidOperationException(message);
            }

            if (fileItem.RecognitionState != RecognitionState.Prepared)
                return;

            try
            {
                await _applicationLogService.InfoAsync($"Speech recognition is started for file ID: '{fileItem.Id}'.", userId).ConfigureAwait(false);
                await RunRecognitionInternalAsync(fileItem).ConfigureAwait(false);
                await _applicationLogService.InfoAsync($"Speech recognition is completed for file ID: '{fileItem.Id}'.", userId).ConfigureAwait(false);

                await _fileItemService.RemoveSourceFileNameAsync(fileItem).ConfigureAwait(false);
            }
            catch
            {
                await _applicationLogService.InfoAsync($"Speech recognition is not successful for file ID: '{fileItem.Id}'.", userId).ConfigureAwait(false);
                throw;
            }
        }

        private async Task RunRecognitionInternalAsync(FileItem fileItem)
        {
            var remainingTime = await _userSubscriptionService.GetRemainingTime(fileItem.UserId).ConfigureAwait(false);
            await _fileItemService.UpdateRecognitionStateAsync(fileItem.Id, RecognitionState.InProgress, _appSettings.ApplicationId).ConfigureAwait(false);

            var audioSource = await _fileItemService.GetAudioSource(fileItem.Id).ConfigureAwait(false);
            var wavFiles = await _wavFileService.SplitWavFileAsync(audioSource, remainingTime).ConfigureAwait(false);
            var files = wavFiles.ToList();

            var transcribedTime = files.OrderByDescending(x => x.EndTime).FirstOrDefault()?.EndTime ?? TimeSpan.Zero;
            await _fileItemService.UpdateTranscribedTimeAsync(fileItem.Id, transcribedTime).ConfigureAwait(false);

            try
            {
                var transcribeItems = await _speechRecognitionService.Recognize(fileItem, files).ConfigureAwait(false);
                await _transcribeItemService.AddAsync(transcribeItems).ConfigureAwait(false);

                await _fileItemService.UpdateDateProcessedAsync(fileItem.Id, _appSettings.ApplicationId).ConfigureAwait(false);
                await _fileItemService.UpdateRecognitionStateAsync(fileItem.Id, RecognitionState.Completed, _appSettings.ApplicationId).ConfigureAwait(false);
            }
            finally
            {
                DeleteTempFiles(files);
            }
        }

        private void DeleteTempFiles(IEnumerable<WavPartialFile> files)
        {
            foreach (var file in files)
            {
                if (File.Exists(file.Path))
                    File.Delete(file.Path);
            }
        }
    }
}
