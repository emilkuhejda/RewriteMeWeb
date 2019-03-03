using System;
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

        public async Task<byte[]> ConvertToWav(byte[] source)
        {
            var inputFile = Path.GetTempFileName();
            await File.WriteAllBytesAsync(inputFile, source).ConfigureAwait(false);

            var outputFile = GetTempName();
            await Task.Run(() =>
            {
                using (var reader = new MediaFoundationReader(inputFile))
                {
                    WaveFileWriter.CreateWaveFile(outputFile, reader);
                }
            }).ConfigureAwait(false);

            var bytes = await File.ReadAllBytesAsync(outputFile).ConfigureAwait(false);

            File.Delete(inputFile);
            File.Delete(outputFile);

            return bytes;
        }

        public async Task<IEnumerable<WavFileItem>> SplitWavFile(byte[] inputFile)
        {
            return await Task.Run(() => SplitWavFileInternal(inputFile)).ConfigureAwait(false);
        }

        private IEnumerable<WavFileItem> SplitWavFileInternal(byte[] inputFile)
        {
            var files = new List<WavFileItem>();

            using (var stream = new MemoryStream(inputFile))
            using (var reader = new WaveFileReader(stream))
            {
                var countItems = (int)Math.Floor(reader.TotalTime.TotalSeconds / FileLengthInSeconds);

                for (var i = 0; i < countItems; i++)
                {
                    var startTime = new TimeSpan(0, 0, i * FileLengthInSeconds);
                    var endTile = new TimeSpan(0, 0, (i + 1) * FileLengthInSeconds);
                    TrimWavFile(reader, startTime, endTile, files);
                }

                var start = new TimeSpan(0, 0, countItems * FileLengthInSeconds);
                var end = new TimeSpan(0, 0, (int)reader.TotalTime.TotalSeconds);
                TrimWavFile(reader, start, end, files);

                return files;
            }
        }

        private void TrimWavFile(WaveFileReader reader, TimeSpan start, TimeSpan end, IList<WavFileItem> files)
        {
            var outputFileName = GetTempName();
            var fileItem = TrimWavFile(reader, outputFileName, start, end);
            files.Add(fileItem);
        }

        private WavFileItem TrimWavFile(WaveFileReader reader, string outputFileName, TimeSpan start, TimeSpan end)
        {
            using (var writer = new WaveFileWriter(outputFileName, reader.WaveFormat))
            {
                var segement = reader.WaveFormat.AverageBytesPerSecond / 1000;

                var startPosition = (int)start.TotalMilliseconds * segement;
                startPosition = startPosition - startPosition % reader.WaveFormat.BlockAlign;

                var endPosition = (int)end.TotalMilliseconds * segement;
                endPosition = endPosition - endPosition % reader.WaveFormat.BlockAlign;

                TrimWavFile(reader, writer, startPosition, endPosition);

                return new WavFileItem(outputFileName, writer.TotalTime);
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
