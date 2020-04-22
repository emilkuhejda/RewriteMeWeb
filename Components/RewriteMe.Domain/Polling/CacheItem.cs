using System;
using RewriteMe.Domain.Enums;

namespace RewriteMe.Domain.Polling
{
    public class CacheItem
    {
        public CacheItem()
        {
        }

        public CacheItem(Guid userId, Guid fileItem, RecognitionState recognitionState)
        {
            UserId = userId;
            FileItem = fileItem;
            RecognitionState = recognitionState;
        }

        public Guid UserId { get; private set; }

        public Guid FileItem { get; private set; }

        public RecognitionState RecognitionState { get; set; }

        public double PercentageDone { get; set; }

        public CacheItem Copy()
        {
            return new CacheItem
            {
                UserId = UserId,
                FileItem = FileItem,
                RecognitionState = RecognitionState,
                PercentageDone = PercentageDone
            };
        }
    }
}
