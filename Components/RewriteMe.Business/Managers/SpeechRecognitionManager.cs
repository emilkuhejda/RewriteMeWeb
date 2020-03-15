using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.Rest;
using RewriteMe.Business.Configuration;
using RewriteMe.Business.InformationMessages;
using RewriteMe.Common.Helpers;
using RewriteMe.Common.Utils;
using RewriteMe.Domain.Enums;
using RewriteMe.Domain.Exceptions;
using RewriteMe.Domain.Interfaces.Managers;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.Domain.Settings;
using RewriteMe.Domain.Transcription;
using Serilog;

namespace RewriteMe.Business.Managers
{
    public class SpeechRecognitionManager : ISpeechRecognitionManager
    {
        private static readonly SemaphoreSlim SemaphoreSlim = new SemaphoreSlim(1, 1);

        private readonly ISpeechRecognitionService _speechRecognitionService;
        private readonly IFileItemService _fileItemService;
        private readonly ITranscribeItemService _transcribeItemService;
        private readonly IUserSubscriptionService _userSubscriptionService;
        private readonly ITranscribeItemSourceService _transcribeItemSourceService;
        private readonly IInformationMessageService _informationMessageService;
        private readonly IInternalValueService _internalValueService;
        private readonly IPushNotificationsService _pushNotificationsService;
        private readonly IWavFileManager _wavFileManager;
        private readonly AppSettings _appSettings;
        private readonly ILogger _logger;

        public SpeechRecognitionManager(
            ISpeechRecognitionService speechRecognitionService,
            IFileItemService fileItemService,
            ITranscribeItemService transcribeItemService,
            IUserSubscriptionService userSubscriptionService,
            ITranscribeItemSourceService transcribeItemSourceService,
            IInformationMessageService informationMessageService,
            IInternalValueService internalValueService,
            IPushNotificationsService pushNotificationsService,
            IWavFileManager wavFileManager,
            IOptions<AppSettings> options,
            ILogger logger)
        {
            _speechRecognitionService = speechRecognitionService;
            _fileItemService = fileItemService;
            _transcribeItemService = transcribeItemService;
            _userSubscriptionService = userSubscriptionService;
            _transcribeItemSourceService = transcribeItemSourceService;
            _informationMessageService = informationMessageService;
            _internalValueService = internalValueService;
            _pushNotificationsService = pushNotificationsService;
            _wavFileManager = wavFileManager;
            _appSettings = options.Value;
            _logger = logger;
        }

        public async Task<bool> CanRunRecognition(Guid userId)
        {
            var subscriptionRemainingTime = await _userSubscriptionService.GetRemainingTimeAsync(userId).ConfigureAwait(false);
            return subscriptionRemainingTime.TotalSeconds > 15;
        }

        public void RunRecognition(Guid userId, Guid fileItemId)
        {
            AsyncHelper.RunSync(() => RunRecognitionAsync(userId, fileItemId));
        }

        private async Task RunRecognitionAsync(Guid userId, Guid fileItemId)
        {
            var fileItem = await _fileItemService.GetAsync(userId, fileItemId).ConfigureAwait(false);
            if (fileItem.RecognitionState > RecognitionState.Prepared)
            {
                _logger.Warning($"File with ID: '{fileItem.Id}' is already recognized.");

                return;
            }

            await _wavFileManager.RunConversionToWavAsync(fileItem, userId).ConfigureAwait(false);

            _logger.Information($"Attempt to start Speech recognition for file ID: '{fileItem.Id}'.");
            if (fileItem.RecognitionState < RecognitionState.Prepared)
            {
                var message = $"File with ID: '{fileItem.Id}' is still converting. Speech recognition is stopped.";
                _logger.Warning(message);

                throw new InvalidOperationException(message);
            }

            var canRunRecognition = await CanRunRecognition(fileItem.UserId).ConfigureAwait(false);
            if (!canRunRecognition)
            {
                var message = $"User ID = '{fileItem.UserId}' does not have enough free minutes in the subscription.";
                _logger.Warning(message);

                throw new InvalidOperationException(message);
            }

            if (fileItem.RecognitionState != RecognitionState.Prepared)
                return;

            try
            {
                _logger.Information($"Speech recognition is started for file ID: '{fileItem.Id}'.");
                await RunRecognitionInternalAsync(userId, fileItem).ConfigureAwait(false);
                _logger.Information($"Speech recognition is completed for file ID: '{fileItem.Id}'.");

                await _fileItemService.RemoveSourceFileAsync(fileItem).ConfigureAwait(false);
            }
            catch (FileItemNotExistsException)
            {
                _logger.Warning($"Speech recognition is stopped because file with ID: '{fileItem.Id}' is not found.");

                return;
            }
            catch (FileItemIsNotInPreparedStateException)
            {
                _logger.Warning($"Speech recognition is stopped because file with ID: '{fileItem.Id}' is not in PREPARED state.");

                return;
            }
            catch (Exception ex)
            {
                _logger.Error($"Speech recognition is not successful for file ID: '{fileItem.Id}'.");
                _logger.Error(ExceptionFormatter.FormatException(ex));

                throw;
            }

            await SendNotificationsAsync(userId, fileItemId).ConfigureAwait(false);
        }

        private async Task RunRecognitionInternalAsync(Guid userId, FileItem fileItem)
        {
            await SemaphoreSlim.WaitAsync().ConfigureAwait(true);
            try
            {
                var isInPreparedState = await _fileItemService.IsInPreparedStateAsync(fileItem.Id).ConfigureAwait(true);
                if (!isInPreparedState)
                    throw new FileItemIsNotInPreparedStateException();

                await _fileItemService.UpdateRecognitionStateAsync(fileItem.Id, RecognitionState.InProgress, _appSettings.ApplicationId).ConfigureAwait(true);
            }
            finally
            {
                SemaphoreSlim.Release();
            }

            var remainingTime = await _userSubscriptionService.GetRemainingTimeAsync(fileItem.UserId).ConfigureAwait(false);
            var wavFiles = await _wavFileManager.SplitFileItemSourceAsync(fileItem, remainingTime).ConfigureAwait(false);
            var files = wavFiles.ToList();

            if (fileItem.Storage == StorageSetting.Database ||
                await _internalValueService.GetValueAsync(InternalValues.IsDatabaseBackupEnabled).ConfigureAwait(false))
            {
                await _transcribeItemSourceService.AddWavFileSourcesAsync(fileItem.Id, files).ConfigureAwait(false);
            }

            var transcribedTime = files.OrderByDescending(x => x.EndTime).FirstOrDefault()?.EndTime ?? TimeSpan.Zero;
            await _fileItemService.UpdateTranscribedTimeAsync(fileItem.Id, transcribedTime).ConfigureAwait(false);

            try
            {
                var transcribeItems = await _speechRecognitionService.RecognizeAsync(fileItem, files).ConfigureAwait(false);
                await _transcribeItemService.AddAsync(transcribeItems).ConfigureAwait(false);

                var transcriptionTimeTicks = files.Sum(x => x.TotalTime.Ticks);
                var transcriptionTime = TimeSpan.FromTicks(transcriptionTimeTicks);
                await _userSubscriptionService.SubtractTimeAsync(userId, transcriptionTime).ConfigureAwait(false);

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

        private async Task SendNotificationsAsync(Guid userId, Guid fileItemId)
        {
            var isProgressNotificationsEnabled = await _internalValueService.GetValueAsync(InternalValues.IsProgressNotificationsEnabled).ConfigureAwait(false);
            if (!isProgressNotificationsEnabled)
                return;

            var informationMessage = GenericNotifications.GetTranscriptionSuccess(userId, fileItemId);

            await _informationMessageService.AddAsync(informationMessage).ConfigureAwait(false);
            try
            {
                var tasks = new[]
                {
                    _pushNotificationsService.SendAsync(informationMessage, RuntimePlatform.Android, Language.English, userId),
                    _pushNotificationsService.SendAsync(informationMessage, RuntimePlatform.Android, Language.Slovak, userId),
                    _pushNotificationsService.SendAsync(informationMessage, RuntimePlatform.Osx, Language.English, userId),
                    _pushNotificationsService.SendAsync(informationMessage, RuntimePlatform.Osx, Language.Slovak, userId)
                };

                await Task.WhenAll(tasks).ConfigureAwait(false);
            }
            catch (SerializationException ex)
            {
                _logger.Error($"Request exception during sending notification with message: '{ex.Message}'");
                _logger.Error(ExceptionFormatter.FormatException(ex));
            }
            catch (NotificationErrorException ex)
            {
                _logger.Error($"Request exception during sending notification with message: '{ex.NotificationError.Message}'");
                _logger.Error(ExceptionFormatter.FormatException(ex));
            }
            catch (LanguageVersionNotExistsException)
            {
                _logger.Error($"Language version not found for information message with ID = '{informationMessage.Id}'.");
            }
            catch (PushNotificationWasSentException)
            {
                _logger.Error($"Push notification was already sent for information message with ID = '{informationMessage.Id}'.");
            }
        }
    }
}
