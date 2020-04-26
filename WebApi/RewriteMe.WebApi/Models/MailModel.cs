using System;

namespace RewriteMe.WebApi.Models
{
    public class MailModel
    {
        public Guid FileItemId { get; set; }

        public string Recipient { get; set; }
    }
}
