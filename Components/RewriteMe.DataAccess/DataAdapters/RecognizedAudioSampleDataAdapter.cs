using System.Linq;
using RewriteMe.DataAccess.Entities;
using RewriteMe.Domain.Recording;

namespace RewriteMe.DataAccess.DataAdapters
{
    public static class RecognizedAudioSampleDataAdapter
    {
        public static RecognizedAudioSample ToRecognizedAudioSample(this RecognizedAudioSampleEntity entity)
        {
            return new RecognizedAudioSample
            {
                Id = entity.Id,
                UserId = entity.UserId,
                DateCreatedUtc = entity.DateCreatedUtc,
                SpeechResults = entity.SpeechResults?.Select(x => x.ToSpeechResult())
            };
        }

        public static RecognizedAudioSampleEntity ToRecognizedAudioSampleEntity(this RecognizedAudioSample recognizedAudioSample)
        {
            return new RecognizedAudioSampleEntity
            {
                Id = recognizedAudioSample.Id,
                UserId = recognizedAudioSample.UserId,
                DateCreatedUtc = recognizedAudioSample.DateCreatedUtc,
                SpeechResults = recognizedAudioSample.SpeechResults?.Select(x => x.ToSpeechResultEntity()).ToList()
            };
        }
    }
}
