using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Speech.V1;
using Grpc.Auth;
using Newtonsoft.Json;
using RewriteMe.Domain.Extensions;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.Domain.Settings;
using RewriteMe.Domain.Transcription;

namespace RewriteMe.Business.Services
{
    public class SpeechRecognitionService : ISpeechRecognitionService
    {
        private readonly IWavFileService _wavFileService;
        private readonly ITranscribeItemService _transcribeItemService;

        public SpeechRecognitionService(
            IWavFileService wavFileService,
            ITranscribeItemService transcribeItemService)
        {
            _wavFileService = wavFileService;
            _transcribeItemService = transcribeItemService;
        }

        public async Task Recognize(FileItem fileItem, SpeechCredentials speechCredentials)
        {
            if (!fileItem.IsSupportedType())
                throw new InvalidOperationException("File type is not supported");

            var audioSource = fileItem.IsWav()
                ? fileItem.Source
                : await _wavFileService.ConvertToWav(fileItem.Source).ConfigureAwait(false);

            var wavFiles = await _wavFileService.SplitWavFile(audioSource).ConfigureAwait(false);
            var files = wavFiles.ToList();

            var serializedCredentials = JsonConvert.SerializeObject(speechCredentials);
            var credentials = GoogleCredential.FromJson(serializedCredentials);
            if (credentials.IsCreateScopedRequired)
            {
                credentials = credentials.CreateScoped("https://www.googleapis.com/auth/cloud-platform");
            }

            var channel = new Grpc.Core.Channel(SpeechClient.DefaultEndpoint.Host, credentials.ToChannelCredentials());
            var speechClient = SpeechClient.Create(channel);

            var task = new List<Task>();
            foreach (var file in files)
            {
                task.Add(RecognizeSpeech(speechClient, fileItem.Id, file));
            }

            await Task.WhenAll(task).ConfigureAwait(false);

            foreach (var file in files)
            {
                if (File.Exists(file.Path))
                    File.Delete(file.Path);
            }

            await Task.CompletedTask.ConfigureAwait(false);
        }

        private async Task RecognizeSpeech(SpeechClient speech, Guid fileItemId, WavFileItem wavFileItem)
        {
            await Task.Run(async () =>
            {
                var longOperation = speech.LongRunningRecognize(new RecognitionConfig
                {
                    Encoding = RecognitionConfig.Types.AudioEncoding.Linear16,
                    LanguageCode = "en-GB"
                }, RecognitionAudio.FromFile(wavFileItem.Path));

                longOperation = longOperation.PollUntilCompleted();
                var response = longOperation.Result;

                var alternatives = response.Results
                    .SelectMany(x => x.Alternatives)
                    .Select(x => new RecognitionAlternative(x.Transcript, x.Confidence));

                var source = await File.ReadAllBytesAsync(wavFileItem.Path).ConfigureAwait(false);
                var transcribeItem = new TranscribeItem
                {
                    Id = Guid.NewGuid(),
                    FileItemId = fileItemId,
                    Alternatives = alternatives,
                    Source = source,
                    TotalTime = wavFileItem.TotalTime,
                    DateCreated = DateTime.UtcNow
                };

                await _transcribeItemService.AddAsync(transcribeItem).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }
    }
}
