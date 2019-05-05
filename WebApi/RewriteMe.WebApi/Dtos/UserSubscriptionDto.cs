using System;

namespace RewriteMe.WebApi.Dtos
{
    public class UserSubscriptionDto
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public string TimeString { get; set; }

        public DateTime DateCreated { get; set; }
    }
}
