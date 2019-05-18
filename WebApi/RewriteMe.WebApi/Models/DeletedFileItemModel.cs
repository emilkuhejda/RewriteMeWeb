using System;
using System.ComponentModel.DataAnnotations;

namespace RewriteMe.WebApi.Models
{
    public class DeletedFileItemModel
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public DateTime DeletedDate { get; set; }
    }
}
