using System;
using RewriteMe.Domain.Enums;

namespace RewriteMe.Domain.Polling
{
    public class CacheItem
    {
        public CacheItem(Guid userId, Guid fileItemId, RecognitionState recognitionState)
        {
            UserId = userId;
            FileItemId = fileItemId;
            RecognitionState = recognitionState;
        }

        public static CacheItem Empty => new CacheItem(Guid.Empty, Guid.Empty, RecognitionState.None);

        public Guid UserId { get; }

        public Guid FileItemId { get; }

        public RecognitionState RecognitionState { get; set; }

        public double PercentageDone { get; set; }
    }
}
