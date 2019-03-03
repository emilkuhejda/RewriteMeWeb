using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Speech.V1;
using Grpc.Auth;
using Newtonsoft.Json;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.Domain.Settings;

namespace RewriteMe.Business.Services
{
    public class SpeechRecognitionService : ISpeechRecognitionService
    {
        private readonly IWavFileService _wavFileService;

        public SpeechRecognitionService(IWavFileService wavFileService)
        {
            _wavFileService = wavFileService;
        }

        public async Task Recognize(byte[] audioFile, SpeechCredentials speechCredentials)
        {
            var wavFiles = await _wavFileService.SplitWavFile(audioFile).ConfigureAwait(false);
            var files = wavFiles.ToList();

            var serializedCredentials = JsonConvert.SerializeObject(speechCredentials);
            var credentials = GoogleCredential.FromJson(serializedCredentials);
            if (credentials.IsCreateScopedRequired)
            {
                credentials = credentials.CreateScoped("https://www.googleapis.com/auth/cloud-platform");
            }

            var channel = new Grpc.Core.Channel(SpeechClient.DefaultEndpoint.Host, credentials.ToChannelCredentials());
            var speech = SpeechClient.Create(channel);

            var longOperation = speech.LongRunningRecognize(new RecognitionConfig()
            {
                Encoding = RecognitionConfig.Types.AudioEncoding.Linear16,
                LanguageCode = "en-GB"
            }, RecognitionAudio.FromFile(files[1]));
            longOperation = longOperation.PollUntilCompleted();
            var response = longOperation.Result;

            foreach (var r in response.Results)
            {
                foreach (var alternative in r.Alternatives)
                {
                    Console.WriteLine($"Transcript: { alternative.Transcript}");
                }
            }

            foreach (var file in files)
            {
                if (File.Exists(file))
                    File.Delete(file);
            }

            await Task.CompletedTask.ConfigureAwait(false);
        }
    }
}
