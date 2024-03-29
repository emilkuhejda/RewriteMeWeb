﻿using RewriteMe.Domain.Recording;
using RewriteMe.WebApi.Models;

namespace RewriteMe.WebApi.Extensions
{
    public static class CreateSpeechResultModelExtensions
    {
        public static SpeechResult ToSpeechResult(this CreateSpeechResultModel createSpeechResultModel)
        {
            return new SpeechResult
            {
                Id = createSpeechResultModel.SpeechResultId,
                RecognizedAudioSampleId = createSpeechResultModel.RecognizedAudioSampleId,
                DisplayText = createSpeechResultModel.DisplayText
            };
        }
    }
}
