using RewriteMe.Domain.Dtos;
using RewriteMe.Domain.Polling;

namespace RewriteMe.Business.Extensions
{
    public static class CacheItemExtensions
    {
        public static CacheItemDto ToDto(this CacheItem cacheItem)
        {
            return new CacheItemDto
            {
                FileItemId = cacheItem.FileItemId,
                RecognitionState = cacheItem.RecognitionState,
                PercentageDone = cacheItem.PercentageDone
            };
        }
    }
}
