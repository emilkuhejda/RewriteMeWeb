using System;
using System.ComponentModel.DataAnnotations;

namespace RewriteMe.Domain.Models
{
    public class DeletedFileItemModel
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public DateTime DeletedDate { get; set; }
    }
}
