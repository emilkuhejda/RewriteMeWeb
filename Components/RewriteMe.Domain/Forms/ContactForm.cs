using System;

namespace RewriteMe.Domain.Forms
{
    public class ContactForm
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Message { get; set; }

        public DateTime DateCreatedUtc { get; set; }
    }
}
