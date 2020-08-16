using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Speech.V1;
using Grpc.Auth;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RewriteMe.Business.Configuration;
using RewriteMe.Business.Extensions;
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
        private readonly ICacheService _cacheService;
        private readonly ITranscribeItemService _transcribeItemService;
        private readonly IWavPartialFileService _wavPartialFileService;
        private readonly IInternalValueService _internalValueService;
        private readonly IFileAccessService _fileAccessService;
        private readonly AppSettings _appSettings;
        private readonly ILogger _logger;

        private int _totalTasks;
        private int _tasksDone;

        public SpeechRecognitionService(
            ICacheService cacheService,
            ITranscribeItemService transcribeItemService,
            IWavPartialFileService wavPartialFileService,
            IInternalValueService internalValueService,
            IFileAccessService fileAccessService,
            IOptions<AppSettings> options,
            ILogger logger)
        {
            _cacheService = cacheService;
            _transcribeItemService = transcribeItemService;
            _wavPartialFileService = wavPartialFileService;
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

        public async Task RecognizeAsync(FileItem fileItem, IEnumerable<WavPartialFile> files)
        {
            var speechClient = CreateSpeechClient();
            var storageSetting = await _internalValueService.GetValueAsync(InternalValues.StorageSetting).ConfigureAwait(false);

            var updateMethods = new List<Func<Task<TranscribeItem>>>();
            foreach (var file in files)
            {
                updateMethods.Add(() => RecognizeSpeech(speechClient, fileItem.UserId, fileItem.Id, fileItem.Language, file, storageSetting));
            }

            _totalTasks = updateMethods.Count;
            _tasksDone = 0;

            foreach (var enumerable in updateMethods.Split(10))
            {
                var tasks = enumerable.WhenTaskDone(async () => await UpdateCache(fileItem.Id).ConfigureAwait(false)).Select(x => x());
                await Task.WhenAll(tasks).ConfigureAwait(false);
            }
        }

        private async Task UpdateCache(Guid fileItemId)
        {
            var currentTask = Interlocked.Increment(ref _tasksDone);
            var percentageDone = (int)((double)currentTask / _totalTasks * 100);
            await _cacheService.UpdatePercentageAsync(fileItemId, percentageDone).ConfigureAwait(false);
        }

        private SpeechClient CreateSpeechClient()
        {
            _logger.Information("Create speech recognition client.");

            var serializedCredentials = JsonConvert.SerializeObject(_appSettings.SpeechCredentials);
            var credentials = GoogleCredential
                .FromJson(serializedCredentials)
                .CreateScoped(_appSettings.GoogleApiAuthUri);

            var builder = new SpeechClientBuilder
            {
                ChannelCredentials = credentials.ToChannelCredentials()
            };

            return builder.Build();
        }

        private async Task<TranscribeItem> RecognizeSpeech(SpeechClient speech, Guid userId, Guid fileItemId, string language, WavPartialFile wavPartialFile, StorageSetting storageSetting)
        {
            _logger.Information($"Start recognition for file {wavPartialFile.Path}.");

#if DEBUG
            var client = speech;
            var lang = language;
            var response = new LongRunningRecognizeResponse
            {
                Results =
                {
                    new SpeechRecognitionResult
                    {
                        Alternatives =
                        {
                            new SpeechRecognitionAlternative
                            {
                                Confidence = 0.99f,
                                Transcript = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum."
                            }
                        }
                    }
                }
            };
#else
            var longOperation = speech.LongRunningRecognize(new RecognitionConfig
            {
                Encoding = RecognitionConfig.Types.AudioEncoding.Linear16,
                LanguageCode = language,
                EnableAutomaticPunctuation = true,
                UseEnhanced = true,
                EnableWordTimeOffsets = true,
                AudioChannelCount = wavPartialFile.AudioChannels,
                EnableSeparateRecognitionPerChannel = true
            }, RecognitionAudio.FromFile(wavPartialFile.Path));

            longOperation = await longOperation.PollUntilCompletedAsync().ConfigureAwait(false);
            var response = longOperation.Result;
#endif

            var alternatives = response.Results
                .SelectMany(x => x.Alternatives)
                .Select(x => new RecognitionAlternative(x.Transcript, x.Confidence, x.Words.ToRecognitionWords()));

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

            _logger.Information($"Partial file '{wavPartialFile.Path}' was recognized.");

            await _transcribeItemService.AddAsync(transcribeItem).ConfigureAwait(false);
            await _wavPartialFileService.DeleteAsync(wavPartialFile).ConfigureAwait(false);

            return transcribeItem;
        }

        private string SaveFileToDisk(WavPartialFile wavPartialFile, Guid userId, Guid fileItemId)
        {
            var sourceFileName = Guid.NewGuid().ToString();
            var transcriptionsDirectoryPath = _fileAccessService.GetTranscriptionsDirectoryPath(userId, fileItemId);
            var sourceFilePath = Path.Combine(transcriptionsDirectoryPath, sourceFileName);
            File.Copy(wavPartialFile.Path, sourceFilePath);

            _logger.Information($"Partial file '{wavPartialFile.Path}' was saved to disk.");

            return sourceFileName;
        }
    }
}
