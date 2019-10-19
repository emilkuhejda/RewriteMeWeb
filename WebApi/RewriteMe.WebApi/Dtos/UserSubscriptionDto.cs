using System;
using System.ComponentModel.DataAnnotations;

namespace RewriteMe.WebApi.Dtos
{
    public class UserSubscriptionDto
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public long TimeTicks { get; set; }

        [Required]
        public DateTime DateCreatedUtc { get; set; }
    }
}
