using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Speech.V1;
using Grpc.Auth;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RewriteMe.Common.Utils;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.Domain.Settings;
using RewriteMe.Domain.Transcription;

namespace RewriteMe.Business.Services
{
    public class SpeechRecognitionService : ISpeechRecognitionService
    {
        private readonly IFileAccessService _fileAccessService;
        private readonly IApplicationLogService _applicationLogService;
        private readonly AppSettings _appSettings;

        public SpeechRecognitionService(
            IFileAccessService fileAccessService,
            IApplicationLogService applicationLogService,
            IOptions<AppSettings> options)
        {
            _fileAccessService = fileAccessService;
            _applicationLogService = applicationLogService;
            _appSettings = options.Value;
        }

        public async Task<bool> CanCreateSpeechClientAsync()
        {
            try
            {
                var speechClient = CreateSpeechClient();
                return speechClient != null;
            }
            catch (Exception ex)
            {
                await _applicationLogService.ErrorAsync($"Unable create Speech client.{Environment.NewLine}{ExceptionFormatter.FormatException(ex)}").ConfigureAwait(false);
            }

            return false;
        }

        public async Task<IEnumerable<TranscribeItem>> RecognizeAsync(FileItem fileItem, IEnumerable<WavPartialFile> files)
        {
            var speechClient = CreateSpeechClient();

            var task = new List<Task<TranscribeItem>>();
            foreach (var file in files)
            {
                task.Add(RecognizeSpeech(speechClient, fileItem.Id, fileItem.Language, file));
            }

            return await Task.WhenAll(task).ConfigureAwait(false);
        }

        private SpeechClient CreateSpeechClient()
        {
            var serializedCredentials = JsonConvert.SerializeObject(_appSettings.SpeechCredentials);
            var credentials = GoogleCredential.FromJson(serializedCredentials);
            if (credentials.IsCreateScopedRequired)
            {
                credentials = credentials.CreateScoped(_appSettings.GoogleApiAuthUri);
            }

            var channel = new Grpc.Core.Channel(SpeechClient.DefaultEndpoint.Host, credentials.ToChannelCredentials());
            return SpeechClient.Create(channel);
        }

        private async Task<TranscribeItem> RecognizeSpeech(SpeechClient speech, Guid fileItemId, string language, WavPartialFile wavPartialFile)
        {
            return await Task.Run(() =>
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

                var sourceFileName = Guid.NewGuid().ToString();
                var transcriptionsDirectoryPath = _fileAccessService.GetTranscriptionsDirectoryPath(fileItemId);
                var sourceFilePath = Path.Combine(transcriptionsDirectoryPath, sourceFileName);
                File.Copy(wavPartialFile.Path, sourceFilePath);

                var dateCreated = DateTime.UtcNow;
                var transcribeItem = new TranscribeItem
                {
                    Id = wavPartialFile.Id,
                    FileItemId = fileItemId,
                    ApplicationId = _appSettings.ApplicationId,
                    Alternatives = alternatives,
                    SourceFileName = sourceFileName,
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
