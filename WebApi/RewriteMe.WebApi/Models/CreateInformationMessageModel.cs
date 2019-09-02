using System.ComponentModel.DataAnnotations;

namespace RewriteMe.WebApi.Models
{
    public class CreateInformationMessageModel
    {
        [Required]
        public string CampaignName { get; set; }

        [Required]
        public string TitleEn { get; set; }

        [Required]
        public string MessageEn { get; set; }

        [Required]
        public string DescriptionEn { get; set; }

        [Required]
        public string TitleSk { get; set; }

        [Required]
        public string MessageSk { get; set; }

        [Required]
        public string DescriptionSk { get; set; }
    }
}
