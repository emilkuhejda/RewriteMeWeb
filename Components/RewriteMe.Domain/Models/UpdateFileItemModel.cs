using System;
using System.ComponentModel.DataAnnotations;

namespace RewriteMe.Domain.Models
{
    public class UpdateFileItemModel
    {
        [Required]
        public Guid FileItemId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Language { get; set; }

        [Required]
        public Guid ApplicationId { get; set; }
    }
}
