using System;
using System.ComponentModel.DataAnnotations;

namespace RewriteMe.WebApi.Dtos
{
    public class AudioSourceDto
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid FileItemId { get; set; }

        [Required]
        [MaxLength(50)]
        public string ContentType { get; set; }

        [Required]
        public int Version { get; set; }
    }
}
