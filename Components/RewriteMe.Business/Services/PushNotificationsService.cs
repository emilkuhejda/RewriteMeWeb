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

namespace RewriteMe.Business.Services
{
    public class PushNotificationsService : IPushNotificationsService
    {
        private const string TargetType = "devices_target";
        private const string MediaType = "application/json";

        private readonly AppSettings _appSettings;

        public PushNotificationsService(IOptions<AppSettings> options)
        {
            _appSettings = options.Value;
        }

        public async Task<NotificationResult> SendAsync(InformationMessage informationMessage, Language language)
        {
            var languageVersion = informationMessage.LanguageVersions.First(x => x.Language == language);
            if (languageVersion == null)
                return null;

            var pushNotification = new PushNotification
            {
                Target = new NotificationTarget
                {
                    Type = TargetType,
                    Devices = new[] { new Guid("04b7bd02-23a2-4ee5-85d2-ba94ce5bb38d") }
                },
                Content = new NotificationContent
                {
                    Name = informationMessage.CampaignName,
                    Title = languageVersion.Title,
                    Body = languageVersion.Message
                }
            };

            using (var result = await SendWithHttpMessagesAsync(pushNotification).ConfigureAwait(false))
            {
                return result.Body;
            }
        }

        private async Task<HttpOperationResponse<NotificationResult>> SendWithHttpMessagesAsync(PushNotification pushNotification, CancellationToken cancellationToken = default(CancellationToken))
        {
            var notificationSettings = _appSettings.NotificationSettings;

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaType));
            httpClient.DefaultRequestHeaders.Add(notificationSettings.ApiKeyName, notificationSettings.AccessToken);

            var content = JsonConvert.SerializeObject(pushNotification);
            var url = $"{notificationSettings.BaseUrl}/{notificationSettings.Organization}/{notificationSettings.AppNameAndroid}/{notificationSettings.Apis}";

            var httpRequest = new HttpRequestMessage
            {
                Method = new HttpMethod("POST"),
                Content = new StringContent(content, Encoding.UTF8, MediaType),
                RequestUri = new Uri(url, UriKind.Absolute)
            };

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
                        var notificationError = JsonConvert.DeserializeObject<NotificationError>(responseContent);
                        throw new NotificationErrorException(notificationError);
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
