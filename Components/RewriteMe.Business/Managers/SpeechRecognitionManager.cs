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
using RewriteMe.Business.Utils;
using RewriteMe.Common.Utils;
using RewriteMe.Domain.Enums;
using RewriteMe.Domain.Exceptions;
using RewriteMe.Domain.Interfaces.Managers;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.Domain.Polling;
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
        private readonly IUserSubscriptionService _userSubscriptionService;
        private readonly ITranscribeItemSourceService _transcribeItemSourceService;
        private readonly IInformationMessageService _informationMessageService;
        private readonly IWavPartialFileService _wavPartialFileService;
        private readonly IInternalValueService _internalValueService;
        private readonly IPushNotificationsService _pushNotificationsService;
        private readonly ICacheService _cacheService;
        private readonly IWavFileManager _wavFileManager;
        private readonly IMessageCenterService _messageCenterService;
        private readonly AppSettings _appSettings;
        private readonly ILogger _logger;

        public SpeechRecognitionManager(
            ISpeechRecognitionService speechRecognitionService,
            IFileItemService fileItemService,
            IUserSubscriptionService userSubscriptionService,
            ITranscribeItemSourceService transcribeItemSourceService,
            IInformationMessageService informationMessageService,
            IWavPartialFileService wavPartialFileService,
            IInternalValueService internalValueService,
            IPushNotificationsService pushNotificationsService,
            ICacheService cacheService,
            IWavFileManager wavFileManager,
            IMessageCenterService messageCenterService,
            IOptions<AppSettings> options,
            ILogger logger)
        {
            _speechRecognitionService = speechRecognitionService;
            _fileItemService = fileItemService;
            _userSubscriptionService = userSubscriptionService;
            _transcribeItemSourceService = transcribeItemSourceService;
            _informationMessageService = informationMessageService;
            _wavPartialFileService = wavPartialFileService;
            _internalValueService = internalValueService;
            _pushNotificationsService = pushNotificationsService;
            _cacheService = cacheService;
            _wavFileManager = wavFileManager;
            _messageCenterService = messageCenterService;
            _appSettings = options.Value;
            _logger = logger.ForContext<SpeechRecognitionManager>();
        }

        public async Task<bool> CanRunRecognition(Guid userId)
        {
            var subscriptionRemainingTime = await _userSubscriptionService.GetRemainingTimeAsync(userId).ConfigureAwait(false);
            return subscriptionRemainingTime.TotalSeconds > 15;
        }

        public async Task RunRecognitionAsync(Guid userId, Guid fileItemId)
        {
            await RunRecognitionAsync(userId, fileItemId, false).ConfigureAwait(false);
        }

        public async Task RunRecognitionAsync(Guid userId, Guid fileItemId, bool isRestarted)
        {
            FileItem fileItem;

            await SemaphoreSlim.WaitAsync().ConfigureAwait(true);
            try
            {
                if (ProcessingJobs.AnyJob(userId))
                    return;

                fileItem = await _fileItemService.GetAsync(userId, fileItemId).ConfigureAwait(false);
                if (fileItem == null)
                    return;

                if (!isRestarted && fileItem.RecognitionState > RecognitionState.Prepared)
                {
                    _logger.Warning($"File with ID: '{fileItem.Id}' is already recognized. [{fileItem.UserId}]");

                    return;
                }

                ProcessingJobs.Add(userId, fileItemId);
            }
            finally
            {
                SemaphoreSlim.Release();
            }

            try
            {
                _cacheService.RemoveItem(fileItemId);

                await RunRecognitionInternalAsync(fileItem, isRestarted).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                await _fileItemService.UpdateRecognitionStateAsync(fileItemId, RecognitionState.None, _appSettings.ApplicationId).ConfigureAwait(false);
                await _cacheService.UpdateRecognitionStateAsync(fileItemId, RecognitionState.None).ConfigureAwait(false);

                _logger.Error($"Exception occurred during recognition. File item ID = '{fileItemId}'.");
                _logger.Error(ExceptionFormatter.FormatException(ex));

                await _messageCenterService.SendAsync(HubMethodsHelper.GetRecognitionErrorMethod(userId), fileItem.Name).ConfigureAwait(false);
            }
            finally
            {
                _cacheService.RemoveItem(fileItemId);

                ProcessingJobs.Remove(fileItemId);
            }
        }

        private async Task RunRecognitionInternalAsync(FileItem fileItem, bool isRestarted)
        {
            var canRunRecognition = await CanRunRecognition(fileItem.UserId).ConfigureAwait(false);
            if (!canRunRecognition)
            {
                var message = $"User ID = '{fileItem.UserId}' does not have enough free minutes in the subscription. [{fileItem.UserId}]";
                _logger.Warning(message);

                throw new InvalidOperationException(message);
            }

            var cacheItem = new CacheItem(fileItem.UserId, fileItem.Id, fileItem.RecognitionState);
            _cacheService.AddItemAsync(cacheItem).GetAwaiter().GetResult();

            if (!isRestarted || fileItem.RecognitionState == RecognitionState.Converting)
            {
                await _wavFileManager.RunConversionToWavAsync(fileItem, fileItem.UserId).ConfigureAwait(false);

                _logger.Information($"Attempt to start Speech recognition for file ID: '{fileItem.Id}'. [{fileItem.UserId}]");
                if (fileItem.RecognitionState < RecognitionState.Prepared)
                {
                    var message = $"File with ID: '{fileItem.Id}' is still converting. Speech recognition is stopped. [{fileItem.UserId}]";
                    _logger.Warning(message);

                    throw new InvalidOperationException(message);
                }

                if (fileItem.RecognitionState != RecognitionState.Prepared)
                    return;
            }
            else
            {
                _logger.Information($"Attempt to restart Speech recognition for file ID: '{fileItem.Id}'. [{fileItem.UserId}]");

                await _fileItemService.UpdateRecognitionStateAsync(fileItem.Id, RecognitionState.Prepared, _appSettings.ApplicationId).ConfigureAwait(true);
            }

            try
            {
                _logger.Information($"Speech recognition is started for file ID: '{fileItem.Id}'. [{fileItem.UserId}]");
                await RunRecognitionInternalAsync(fileItem.UserId, fileItem, isRestarted).ConfigureAwait(false);
                _logger.Information($"Speech recognition is completed for file ID: '{fileItem.Id}'. [{fileItem.UserId}]");

                await _fileItemService.RemoveSourceFileAsync(fileItem).ConfigureAwait(false);
            }
            catch (FileItemNotExistsException)
            {
                _logger.Warning($"Speech recognition is stopped because file with ID: '{fileItem.Id}' is not found. [{fileItem.UserId}]");

                return;
            }
            catch (FileItemIsNotInPreparedStateException)
            {
                _logger.Warning($"Speech recognition is stopped because file with ID: '{fileItem.Id}' is not in PREPARED state. [{fileItem.UserId}]");

                return;
            }
            catch (Exception ex)
            {
                _logger.Error($"Speech recognition is not successful for file ID: '{fileItem.Id}'. [{fileItem.UserId}]");
                _logger.Error(ExceptionFormatter.FormatException(ex));

                throw;
            }

            await SendNotificationsAsync(fileItem.UserId, fileItem.Id).ConfigureAwait(false);
        }

        private async Task RunRecognitionInternalAsync(Guid userId, FileItem fileItem, bool isRestarted)
        {
            await SemaphoreSlim.WaitAsync().ConfigureAwait(true);
            try
            {
                var isInPreparedState = await _fileItemService.IsInPreparedStateAsync(fileItem.Id).ConfigureAwait(true);
                if (!isInPreparedState)
                    throw new FileItemIsNotInPreparedStateException();

                await _fileItemService.UpdateRecognitionStateAsync(fileItem.Id, RecognitionState.InProgress, _appSettings.ApplicationId).ConfigureAwait(true);
                await _cacheService.UpdateRecognitionStateAsync(fileItem.Id, RecognitionState.InProgress).ConfigureAwait(false);
            }
            finally
            {
                SemaphoreSlim.Release();
            }

            var remainingTime = await _userSubscriptionService.GetRemainingTimeAsync(fileItem.UserId).ConfigureAwait(false);

            IList<WavPartialFile> files;
            if (isRestarted)
            {
                var partialFiles = (await _wavPartialFileService.GetAsync(fileItem.Id).ConfigureAwait(false)).ToList();
                var allExists = partialFiles.All(x => File.Exists(x.Path));
                files = partialFiles.Any() && allExists
                    ? partialFiles
                    : (await _wavFileManager.SplitFileItemSourceAsync(fileItem, remainingTime).ConfigureAwait(false)).ToList();
            }
            else
            {
                files = (await _wavFileManager.SplitFileItemSourceAsync(fileItem, remainingTime).ConfigureAwait(false)).ToList();
            }

            if (fileItem.Storage == StorageSetting.Database ||
                await _internalValueService.GetValueAsync(InternalValues.IsDatabaseBackupEnabled).ConfigureAwait(false))
            {
                await _transcribeItemSourceService.AddWavFileSourcesAsync(fileItem.Id, files).ConfigureAwait(false);
            }

            var transcribedTime = files.OrderByDescending(x => x.EndTime).FirstOrDefault()?.EndTime ?? TimeSpan.Zero;
            await _fileItemService.UpdateTranscribedTimeAsync(fileItem.Id, transcribedTime).ConfigureAwait(false);

            try
            {
                await _speechRecognitionService.RecognizeAsync(fileItem, files).ConfigureAwait(false);

                var transcriptionTimeTicks = files.Sum(x => x.TotalTime.Ticks);
                var transcriptionTime = TimeSpan.FromTicks(transcriptionTimeTicks);
                await _userSubscriptionService.SubtractTimeAsync(userId, transcriptionTime).ConfigureAwait(false);

                await _fileItemService.UpdateDateProcessedAsync(fileItem.Id, _appSettings.ApplicationId).ConfigureAwait(false);
                await _fileItemService.UpdateRecognitionStateAsync(fileItem.Id, RecognitionState.Completed, _appSettings.ApplicationId).ConfigureAwait(false);
                await _cacheService.UpdateRecognitionStateAsync(fileItem.Id, RecognitionState.Completed).ConfigureAwait(false);
                _wavPartialFileService.DeleteDirectory(userId, fileItem.Id);
            }
            catch (Exception ex)
            {
                _logger.Error($"Exception occurred during recognition process. File ID = '{fileItem.Id}'.");
                _logger.Error(ExceptionFormatter.FormatException(ex));

                throw;
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
                _logger.Error($"Request exception during sending notification with message: '{ex.Message}'. [{userId}]");
                _logger.Error(ExceptionFormatter.FormatException(ex));
            }
            catch (NotificationErrorException ex)
            {
                _logger.Error($"Request exception during sending notification with message: '{ex.NotificationError.Message}'. [{userId}]");
                _logger.Error(ExceptionFormatter.FormatException(ex));
            }
            catch (LanguageVersionNotExistsException)
            {
                _logger.Error($"Language version not found for information message with ID = '{informationMessage.Id}'. [{userId}]");
            }
            catch (PushNotificationWasSentException)
            {
                _logger.Error($"Push notification was already sent for information message with ID = '{informationMessage.Id}'. [{userId}]");
            }
        }
    }
}
