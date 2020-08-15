using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using NAudio.Wave;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.Domain.Transcription;
using Serilog;

namespace RewriteMe.Business.Services
{
    public class WavFileService : IWavFileService
    {
        private const int FileLengthInSeconds = 59;

        private readonly IWavPartialFileService _wavPartialFileService;
        private readonly IFileAccessService _fileAccessService;
        private readonly ILogger _logger;

        public WavFileService(
            IWavPartialFileService wavPartialFileService,
            IFileAccessService fileAccessService,
            ILogger logger)
        {
            _wavPartialFileService = wavPartialFileService;
            _fileAccessService = fileAccessService;
            _logger = logger.ForContext<WavFileService>();
        }

        public async Task<(string outputFilePath, string fileName)> ConvertToWavAsync(string directoryPath, string inputFilePath)
        {
            var fileName = Guid.NewGuid().ToString();
            var outputFilePath = Path.Combine(directoryPath, fileName);
            await Task.Run(() =>
            {
                using (var reader = new MediaFoundationReader(inputFilePath))
                {
                    WaveFileWriter.CreateWaveFile(outputFilePath, reader);
                }
            }).ConfigureAwait(false);

            _logger.Information($"File '{inputFilePath}' was converted. New destination = {outputFilePath}.");

            return (outputFilePath, fileName);
        }

        public async Task<IEnumerable<WavPartialFile>> SplitWavFileAsync(byte[] inputFile, TimeSpan remainingTime, Guid fileItemId, Guid userId)
        {
            return await SplitWavFileInternalAsync(inputFile, remainingTime, fileItemId, userId).ConfigureAwait(false);
        }

        private async Task<IEnumerable<WavPartialFile>> SplitWavFileInternalAsync(byte[] inputFile, TimeSpan remainingTime, Guid fileItemId, Guid userId)
        {
            var files = new List<WavPartialFile>();
            var totalTime = TimeSpan.Zero;

            using (var stream = new MemoryStream(inputFile))
            using (var reader = new WaveFileReader(stream))
            {
                var countItems = (int)Math.Floor(reader.TotalTime.TotalSeconds / FileLengthInSeconds);

                for (var i = 0; i < countItems; i++)
                {
                    var sampleDuration = await ProcessSampleAudioAsync(reader, remainingTime, totalTime, files, fileItemId, userId).ConfigureAwait(false);
                    if (sampleDuration.Ticks <= 0)
                        return files;

                    totalTime = totalTime.Add(sampleDuration);
                }

                await ProcessSampleAudioAsync(reader, remainingTime, totalTime, files, fileItemId, userId).ConfigureAwait(false);

                _logger.Information($"Wav file was split to {files.Count} parts.");

                return files;
            }
        }

        private async Task<TimeSpan> ProcessSampleAudioAsync(WaveFileReader reader, TimeSpan remainingTime, TimeSpan totalTime, IList<WavPartialFile> files, Guid fileItemId, Guid userId)
        {
            var remainingTimeSpan = remainingTime.Subtract(totalTime);
            if (remainingTimeSpan.Ticks <= 0)
                return TimeSpan.MinValue;

            var sampleDuration = remainingTimeSpan.TotalSeconds < FileLengthInSeconds
                ? remainingTimeSpan
                : TimeSpan.FromSeconds(FileLengthInSeconds);

            var audioTotalTime = reader.TotalTime;
            var end = totalTime.Add(sampleDuration);
            var endTime = end > audioTotalTime ? audioTotalTime : end;

            await TrimWavFileAsync(reader, totalTime, endTime, files, fileItemId, userId).ConfigureAwait(false);

            return sampleDuration;
        }

        private async Task TrimWavFileAsync(WaveFileReader reader, TimeSpan start, TimeSpan end, IList<WavPartialFile> files, Guid fileItemId, Guid userId)
        {
            var outputFileName = GetDirectoryPath(userId, fileItemId);
            var fileItem = TrimWavFile(reader, outputFileName, start, end, fileItemId);

            try
            {
                await _wavPartialFileService.AddAsync(fileItem).ConfigureAwait(false);
            }
            catch (Exception)
            {
                if (File.Exists(fileItem.Path))
                {
                    File.Delete(fileItem.Path);
                }

                throw;
            }

            files.Add(fileItem);

            _logger.Information($"Partial wav file was created. File path = {outputFileName}.");
        }

        private WavPartialFile TrimWavFile(WaveFileReader reader, string outputFileName, TimeSpan start, TimeSpan end, Guid fileItemId)
        {
            using (var writer = new WaveFileWriter(outputFileName, reader.WaveFormat))
            {
                var segement = reader.WaveFormat.AverageBytesPerSecond / 1000;

                var startPosition = (int)start.TotalMilliseconds * segement;
                startPosition = startPosition - startPosition % reader.WaveFormat.BlockAlign;

                var endPosition = (int)end.TotalMilliseconds * segement;
                endPosition = endPosition - endPosition % reader.WaveFormat.BlockAlign;

                TrimWavFile(reader, writer, startPosition, endPosition);

                var wavPartialFile = new WavPartialFile
                {
                    Id = Guid.NewGuid(),
                    FileItemId = fileItemId,
                    Path = outputFileName,
                    AudioChannels = reader.WaveFormat.Channels,
                    StartTime = start,
                    EndTime = end,
                    TotalTime = writer.TotalTime
                };

                return wavPartialFile;
            }
        }

        private void TrimWavFile(WaveFileReader reader, WaveFileWriter writer, int startPosition, int endPosition)
        {
            reader.Position = startPosition;
            var buffer = new byte[1024];

            while (reader.Position < endPosition)
            {
                var segment = (int)(endPosition - reader.Position);
                if (segment > 0)
                {
                    var bytesToRead = Math.Min(segment, buffer.Length);
                    var readedBytes = reader.Read(buffer, 0, bytesToRead);
                    if (readedBytes > 0)
                    {
                        writer.Write(buffer, 0, readedBytes);
                    }
                }
            }
        }

        private string GetDirectoryPath(Guid userId, Guid fileItemId)
        {
            var partialFilesDirectoryPath = _fileAccessService.GetPartialFilesDirectoryPath(userId, fileItemId);
            return Path.Combine(partialFilesDirectoryPath, $"{Guid.NewGuid()}.wav");
        }
    }
}
