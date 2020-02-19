using System;
using System.ComponentModel.DataAnnotations;
using RewriteMe.Domain.Enums;

namespace RewriteMe.Domain.Models
{
    public class CreateSubscriptionModel
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        public Guid ApplicationId { get; set; }

        [Required]
        public SubscriptionOperation Operation { get; set; }

        [Required]
        public int Seconds { get; set; }
    }
}
