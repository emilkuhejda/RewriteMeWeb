using RewriteMe.Domain.Enums;

namespace RewriteMe.WebApi.Models
{
    public class CleanUpSettingsModel
    {
        public int DeleteBeforeInDays { get; set; }

        public CleanUpSettings CleanUpSettings { get; set; }

        public bool ForceCleanUp { get; set; }

        public string Password { get; set; }
    }
}
