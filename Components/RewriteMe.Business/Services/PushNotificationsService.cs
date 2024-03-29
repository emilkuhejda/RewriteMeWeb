﻿using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.Rest;
using Newtonsoft.Json;
using RewriteMe.Domain.Enums;
using RewriteMe.Domain.Exceptions;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.Domain.Messages;
using RewriteMe.Domain.Notifications;
using RewriteMe.Domain.Settings;
using Serilog;

namespace RewriteMe.Business.Services
{
    public class PushNotificationsService : IPushNotificationsService
    {
        private const string TargetType = "devices_target";
        private const string MediaType = "application/json";

        private readonly IUserDeviceService _userDeviceService;
        private readonly IInformationMessageService _informationMessageService;
        private readonly ILanguageVersionService _languageVersionService;
        private readonly AppSettings _appSettings;
        private readonly ILogger _logger;

        public PushNotificationsService(
            IUserDeviceService userDeviceService,
            IInformationMessageService informationMessageService,
            ILanguageVersionService languageVersionService,
            IOptions<AppSettings> options,
            ILogger logger)
        {
            _userDeviceService = userDeviceService;
            _informationMessageService = informationMessageService;
            _languageVersionService = languageVersionService;
            _appSettings = options.Value;
            _logger = logger.ForContext<PushNotificationsService>();
        }

        public async Task<NotificationResult> SendAsync(InformationMessage informationMessage, RuntimePlatform runtimePlatform, Language language, Guid? userId = null)
        {
            var languageVersion = informationMessage.LanguageVersions.FirstOrDefault(x => x.Language == language);
            if (languageVersion == null)
                throw new LanguageVersionNotExistsException();

            if (runtimePlatform == RuntimePlatform.Android && languageVersion.SentOnAndroid)
                throw new PushNotificationWasSentException();

            if (runtimePlatform == RuntimePlatform.Osx && languageVersion.SentOnOsx)
                throw new PushNotificationWasSentException();

            NotificationResult notificationResult = null;
            var installationIds = await _userDeviceService.GetPlatformSpecificInstallationIdsAsync(runtimePlatform, language, userId).ConfigureAwait(false);
            var devices = installationIds.ToList();
            if (devices.Any())
            {
                var pushNotification = new PushNotification
                {
                    Target = new NotificationTarget
                    {
                        Type = TargetType,
                        Devices = devices
                    },
                    Content = new NotificationContent
                    {
                        Name = informationMessage.CampaignName,
                        Title = languageVersion.Title,
                        Body = languageVersion.Message
                    }
                };

                using (var result = await SendWithHttpMessagesAsync(pushNotification, runtimePlatform).ConfigureAwait(false))
                {
                    notificationResult = result.Body;
                }
            }

            await _languageVersionService.UpdateSendStatusAsync(languageVersion, runtimePlatform, true).ConfigureAwait(false);
            if (!informationMessage.DatePublishedUtc.HasValue)
            {
                await _informationMessageService.UpdateDatePublishedAsync(informationMessage.Id, DateTime.UtcNow).ConfigureAwait(false);
            }

            _logger.Information($"Notification '{informationMessage.Id}' was sent to runtime platform '{runtimePlatform}' for language '{language}'.");

            return notificationResult;
        }

        private async Task<HttpOperationResponse<NotificationResult>> SendWithHttpMessagesAsync(PushNotification pushNotification, RuntimePlatform runtimePlatform, CancellationToken cancellationToken = default(CancellationToken))
        {
            var notificationSettings = _appSettings.NotificationSettings;

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaType));
            httpClient.DefaultRequestHeaders.Add(notificationSettings.ApiKeyName, notificationSettings.AccessToken);

            var content = JsonConvert.SerializeObject(pushNotification);
            var applicationName = runtimePlatform == RuntimePlatform.Android
                ? notificationSettings.AppNameAndroid
                : notificationSettings.AppNameOsx;
            var url = $"{notificationSettings.BaseUrl}/{notificationSettings.Organization}/{applicationName}/{notificationSettings.Apis}";

            var httpRequest = new HttpRequestMessage
            {
                Method = new HttpMethod("POST"),
                Content = new StringContent(content, Encoding.UTF8, MediaType),
                RequestUri = new Uri(url, UriKind.Absolute)
            };

            _logger.Information($"Send request to url = '{url}'.");

            cancellationToken.ThrowIfCancellationRequested();
            var httpResponse = await httpClient.SendAsync(httpRequest, cancellationToken).ConfigureAwait(false);
            cancellationToken.ThrowIfCancellationRequested();

            var responseContent = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
            var statusCode = httpResponse.StatusCode;
            if (statusCode != HttpStatusCode.Accepted)
            {
                HandleSerialization(
                    () =>
                    {
                        var wrapper = JsonConvert.DeserializeObject<NotificationErrorWrapper>(responseContent);
                        throw new NotificationErrorException(wrapper.Error);
                    },
                    () =>
                    {
                        httpClient.Dispose();
                        httpRequest.Dispose();
                        httpResponse.Dispose();
                    });
            }

            var result = new HttpOperationResponse<NotificationResult>
            {
                Request = httpRequest,
                Response = httpResponse
            };

            HandleSerialization(
                () =>
                {
                    result.Body = JsonConvert.DeserializeObject<NotificationResult>(responseContent);
                },
                () =>
                {
                    httpClient.Dispose();
                    httpRequest.Dispose();
                    httpResponse.Dispose();
                });

            return result;
        }

        private void HandleSerialization(Action serializeAction, Action onFinishedAction)
        {
            try
            {
                serializeAction();
            }
            catch (JsonException ex)
            {
                throw new SerializationException("Unable to deserialize the response.", ex);
            }
            finally
            {
                onFinishedAction();
            }
        }
    }
}
