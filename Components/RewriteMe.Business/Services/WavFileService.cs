﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using NAudio.Wave;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.Domain.Transcription;

namespace RewriteMe.Business.Services
{
    public class WavFileService : IWavFileService
    {
        private const int FileLengthInSeconds = 59;

        private readonly IFileAccessService _fileAccessService;

        public WavFileService(IFileAccessService fileAccessService)
        {
            _fileAccessService = fileAccessService;
        }

        public string CopyWavAsync(FileItem fileItem)
        {
            var inputFile = _fileAccessService.GetOriginalFileItemPath(fileItem);

            var rootDirectoryPath = _fileAccessService.GetRootPath();
            var fileName = Guid.NewGuid().ToString();
            var outputFile = Path.Combine(rootDirectoryPath, fileItem.Id.ToString(), fileName);

            File.Copy(inputFile, outputFile, true);

            return fileName;
        }

        public async Task<string> ConvertToWavAsync(FileItem fileItem)
        {
            var inputFile = _fileAccessService.GetOriginalFileItemPath(fileItem);

            var rootDirectoryPath = _fileAccessService.GetRootPath();
            var fileName = Guid.NewGuid().ToString();
            var outputFile = Path.Combine(rootDirectoryPath, fileItem.Id.ToString(), fileName);
            await Task.Run(() =>
            {
                using (var reader = new MediaFoundationReader(inputFile))
                {
                    WaveFileWriter.CreateWaveFile(outputFile, reader);
                }
            }).ConfigureAwait(false);

            return fileName;
        }

        public async Task<IEnumerable<WavPartialFile>> SplitWavFileAsync(byte[] inputFile, TimeSpan remainingTime)
        {
            return await Task.Run(() => SplitWavFileInternal(inputFile, remainingTime)).ConfigureAwait(false);
        }

        private IEnumerable<WavPartialFile> SplitWavFileInternal(byte[] inputFile, TimeSpan remainingTime)
        {
            var files = new List<WavPartialFile>();
            var totalTime = TimeSpan.Zero;

            using (var stream = new MemoryStream(inputFile))
            using (var reader = new WaveFileReader(stream))
            {
                var countItems = (int)Math.Floor(reader.TotalTime.TotalSeconds / FileLengthInSeconds);

                for (var i = 0; i < countItems; i++)
                {
                    var sampleDuration = ProcessSampleAudio(reader, remainingTime, totalTime, files);
                    if (sampleDuration.Ticks <= 0)
                        return files;

                    totalTime = totalTime.Add(sampleDuration);
                }

                ProcessSampleAudio(reader, remainingTime, totalTime, files);

                return files;
            }
        }

        private TimeSpan ProcessSampleAudio(WaveFileReader reader, TimeSpan remainingTime, TimeSpan totalTime, IList<WavPartialFile> files)
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

            TrimWavFile(reader, totalTime, endTime, files);

            return sampleDuration;
        }

        private void TrimWavFile(WaveFileReader reader, TimeSpan start, TimeSpan end, IList<WavPartialFile> files)
        {
            var outputFileName = GetTempName();
            var fileItem = TrimWavFile(reader, outputFileName, start, end);
            files.Add(fileItem);
        }

        private WavPartialFile TrimWavFile(WaveFileReader reader, string outputFileName, TimeSpan start, TimeSpan end)
        {
            using (var writer = new WaveFileWriter(outputFileName, reader.WaveFormat))
            {
                var segement = reader.WaveFormat.AverageBytesPerSecond / 1000;

                var startPosition = (int)start.TotalMilliseconds * segement;
                startPosition = startPosition - startPosition % reader.WaveFormat.BlockAlign;

                var endPosition = (int)end.TotalMilliseconds * segement;
                endPosition = endPosition - endPosition % reader.WaveFormat.BlockAlign;

                TrimWavFile(reader, writer, startPosition, endPosition);

                return new WavPartialFile
                {
                    Id = Guid.NewGuid(),
                    Path = outputFileName,
                    StartTime = start,
                    EndTime = end,
                    TotalTime = writer.TotalTime
                };
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

        private string GetTempName()
        {
            return Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.wav");
        }
    }
}
