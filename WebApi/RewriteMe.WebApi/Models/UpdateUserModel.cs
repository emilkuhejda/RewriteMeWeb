using System.ComponentModel.DataAnnotations;

namespace RewriteMe.WebApi.Models
{
    public class UpdateUserModel
    {
        [Required]
        public string GivenName { get; set; }

        [Required]
        public string FamilyName { get; set; }
    }
}
