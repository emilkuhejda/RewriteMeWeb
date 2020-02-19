using System.ComponentModel.DataAnnotations;

namespace RewriteMe.Domain.Models
{
    public class UpdateUserModel
    {
        [Required]
        public string GivenName { get; set; }

        [Required]
        public string FamilyName { get; set; }
    }
}
