using System.Collections.Generic;
using System.Linq;
using RewriteMe.Domain.Dtos;
using RewriteMe.Domain.Transcription;

namespace RewriteMe.WebApi.Extensions
{
    public static class RecognitionAlternativeExtensions
    {
        public static IEnumerable<RecognitionAlternativeDto> ToDtos(this IEnumerable<RecognitionAlternative> recognitionAlternatives)
        {
            return recognitionAlternatives.Select(x => new RecognitionAlternativeDto
            {
                Transcript = x.Transcript,
                Confidence = x.Confidence,
                Words = x.Words?.Select(w => w.ToDto()) ?? Enumerable.Empty<RecognitionWordInfoDto>()
            });
        }
    }
}
