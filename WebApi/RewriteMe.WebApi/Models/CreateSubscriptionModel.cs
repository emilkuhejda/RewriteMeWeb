using System;
using System.ComponentModel.DataAnnotations;

namespace RewriteMe.WebApi.Models
{
    public class CreateSubscriptionModel
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        public Guid ApplicationId { get; set; }

        [Required]
        public int Seconds { get; set; }
    }
}
