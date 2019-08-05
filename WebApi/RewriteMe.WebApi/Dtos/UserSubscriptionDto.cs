﻿using System;
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
        [MaxLength(50)]
        public long TimeTicks { get; set; }

        [Required]
        public DateTime DateCreated { get; set; }
    }
}
