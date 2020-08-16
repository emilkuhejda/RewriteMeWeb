using RewriteMe.DataAccess.Entities;
using RewriteMe.Domain.Transcription;

namespace RewriteMe.DataAccess.DataAdapters
{
    public static class WavPartialFileDataAdapter
    {
        public static WavPartialFile ToWavPartialFile(this WavPartialFileEntity partialFileEntity)
        {
            return new WavPartialFile
            {
                Id = partialFileEntity.Id,
                FileItemId = partialFileEntity.FileItemId,
                Path = partialFileEntity.Path,
                AudioChannels = partialFileEntity.AudioChannels,
                StartTime = partialFileEntity.StartTime,
                EndTime = partialFileEntity.EndTime,
                TotalTime = partialFileEntity.TotalTime
            };
        }

        public static WavPartialFileEntity ToWavPartialFileEntity(this WavPartialFile partialFile)
        {
            return new WavPartialFileEntity
            {
                Id = partialFile.Id,
                FileItemId = partialFile.FileItemId,
                Path = partialFile.Path,
                AudioChannels = partialFile.AudioChannels,
                StartTime = partialFile.StartTime,
                EndTime = partialFile.EndTime,
                TotalTime = partialFile.TotalTime
            };
        }
    }
}
