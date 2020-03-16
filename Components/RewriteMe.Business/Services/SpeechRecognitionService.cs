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
using RewriteMe.Business.Configuration;
using RewriteMe.Common.Utils;
using RewriteMe.Domain.Enums;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.Domain.Settings;
using RewriteMe.Domain.Transcription;
using Serilog;

namespace RewriteMe.Business.Services
{
    public class SpeechRecognitionService : ISpeechRecognitionService
    {
        private readonly IInternalValueService _internalValueService;
        private readonly IFileAccessService _fileAccessService;
        private readonly AppSettings _appSettings;
        private readonly ILogger _logger;

        public SpeechRecognitionService(
            IInternalValueService internalValueService,
            IFileAccessService fileAccessService,
            IOptions<AppSettings> options,
            ILogger logger)
        {
            _internalValueService = internalValueService;
            _fileAccessService = fileAccessService;
            _appSettings = options.Value;
            _logger = logger.ForContext<SpeechRecognitionService>();
        }

        public bool CanCreateSpeechClientAsync()
        {
            try
            {
                var speechClient = CreateSpeechClient();
                return speechClient != null;
            }
            catch (Exception ex)
            {
                _logger.Fatal($"Unable create Speech client.");
                _logger.Fatal(ExceptionFormatter.FormatException(ex));
            }

            return false;
        }

        public async Task<IEnumerable<TranscribeItem>> RecognizeAsync(FileItem fileItem, IEnumerable<WavPartialFile> files)
        {
            var speechClient = CreateSpeechClient();
            var storageSetting = await _internalValueService.GetValueAsync(InternalValues.StorageSetting).ConfigureAwait(false);

            var task = new List<Task<TranscribeItem>>();
            foreach (var file in files)
            {
                task.Add(RecognizeSpeech(speechClient, fileItem.UserId, fileItem.Id, fileItem.Language, file, storageSetting));
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

        private async Task<TranscribeItem> RecognizeSpeech(SpeechClient speech, Guid userId, Guid fileItemId, string language, WavPartialFile wavPartialFile, StorageSetting storageSetting)
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

                string sourceFileName = null;
                if (storageSetting == StorageSetting.Disk)
                {
                    sourceFileName = SaveFileToDisk(wavPartialFile, userId, fileItemId);
                }

                var dateCreated = DateTime.UtcNow;
                var transcribeItem = new TranscribeItem
                {
                    Id = wavPartialFile.Id,
                    FileItemId = fileItemId,
                    ApplicationId = _appSettings.ApplicationId,
                    Alternatives = alternatives,
                    SourceFileName = sourceFileName,
                    Storage = storageSetting,
                    StartTime = wavPartialFile.StartTime,
                    EndTime = wavPartialFile.EndTime,
                    TotalTime = wavPartialFile.TotalTime,
                    DateCreatedUtc = dateCreated,
                    DateUpdatedUtc = dateCreated
                };

                return transcribeItem;
            }).ConfigureAwait(false);
        }

        private string SaveFileToDisk(WavPartialFile wavPartialFile, Guid userId, Guid fileItemId)
        {
            var sourceFileName = Guid.NewGuid().ToString();
            var transcriptionsDirectoryPath = _fileAccessService.GetTranscriptionsDirectoryPath(userId, fileItemId);
            var sourceFilePath = Path.Combine(transcriptionsDirectoryPath, sourceFileName);
            File.Copy(wavPartialFile.Path, sourceFilePath);

            return sourceFileName;
        }
    }
}
