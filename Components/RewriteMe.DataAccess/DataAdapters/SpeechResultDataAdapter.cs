using RewriteMe.DataAccess.Entities;
using RewriteMe.Domain.Recording;

namespace RewriteMe.DataAccess.DataAdapters
{
    public static class SpeechResultDataAdapter
    {
        public static SpeechResult ToSpeechResult(this SpeechResultEntity speechResultEntity)
        {
            return new SpeechResult
            {
                Id = speechResultEntity.Id,
                RecognizedAudioSampleId = speechResultEntity.RecognizedAudioSampleId,
                DisplayText = speechResultEntity.DisplayText,
                TotalTime = speechResultEntity.TotalTime
            };
        }

        public static SpeechResultEntity ToSpeechResultEntity(this SpeechResult speechResult)
        {
            return new SpeechResultEntity
            {
                Id = speechResult.Id,
                RecognizedAudioSampleId = speechResult.RecognizedAudioSampleId,
                DisplayText = speechResult.DisplayText,
                TotalTime = speechResult.TotalTime
            };
        }
    }
}
