﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Speech.V1;
using Grpc.Auth;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.Domain.Settings;
using RewriteMe.Domain.Transcription;

namespace RewriteMe.Business.Services
{
    public class SpeechRecognitionService : ISpeechRecognitionService
    {
        private readonly AppSettings _appSettings;

        public SpeechRecognitionService(IOptions<AppSettings> options)
        {
            _appSettings = options.Value;
        }

        public async Task<IEnumerable<TranscribeItem>> Recognize(FileItem fileItem, IEnumerable<WavPartialFile> files)
        {
            var serializedCredentials = JsonConvert.SerializeObject(_appSettings.SpeechCredentials);
            var credentials = GoogleCredential.FromJson(serializedCredentials);
            if (credentials.IsCreateScopedRequired)
            {
                credentials = credentials.CreateScoped(_appSettings.GoogleApiAuthUri);
            }

            var channel = new Grpc.Core.Channel(SpeechClient.DefaultEndpoint.Host, credentials.ToChannelCredentials());
            var speechClient = SpeechClient.Create(channel);

            var task = new List<Task<TranscribeItem>>();
            foreach (var file in files)
            {
                task.Add(RecognizeSpeech(speechClient, fileItem.Id, fileItem.Language, file));
            }

            return await Task.WhenAll(task).ConfigureAwait(false);
        }

        private async Task<TranscribeItem> RecognizeSpeech(SpeechClient speech, Guid fileItemId, string language, WavPartialFile wavPartialFile)
        {
            return await Task.Run(async () =>
            {
                var longOperation = speech.LongRunningRecognize(new RecognitionConfig
                {
                    Encoding = RecognitionConfig.Types.AudioEncoding.Linear16,
                    LanguageCode = language
                }, RecognitionAudio.FromFile(wavPartialFile.Path));

                longOperation = longOperation.PollUntilCompleted();
                var response = longOperation.Result;

                var alternatives = response.Results
                    .SelectMany(x => x.Alternatives)
                    .Select(x => new RecognitionAlternative(x.Transcript, x.Confidence));

                var source = await File.ReadAllBytesAsync(wavPartialFile.Path).ConfigureAwait(false);
                var dateCreated = DateTime.UtcNow;
                var transcribeItem = new TranscribeItem
                {
                    Id = Guid.NewGuid(),
                    FileItemId = fileItemId,
                    ApplicationId = _appSettings.ApplicationId,
                    Alternatives = alternatives,
                    Source = source,
                    StartTime = wavPartialFile.StartTime,
                    EndTime = wavPartialFile.EndTime,
                    TotalTime = wavPartialFile.TotalTime,
                    DateCreated = dateCreated,
                    DateUpdated = dateCreated
                };

                return transcribeItem;
            }).ConfigureAwait(false);
        }
    }
}
