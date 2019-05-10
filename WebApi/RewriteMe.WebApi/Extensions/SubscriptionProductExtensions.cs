using RewriteMe.Domain.Transcription;
using RewriteMe.WebApi.Dtos;

namespace RewriteMe.WebApi.Extensions
{
    public static class SubscriptionProductExtensions
    {
        public static SubscriptionProductDto ToDto(this SubscriptionProduct subscriptionProduct)
        {
            return new SubscriptionProductDto
            {
                Id = subscriptionProduct.Id,
                TimeString = subscriptionProduct.Time.ToString()
            };
        }
    }
}
