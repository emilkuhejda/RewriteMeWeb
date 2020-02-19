using System;
using RewriteMe.Domain.Forms;

namespace RewriteMe.Domain.Extensions
{
    public static class ContactFormModelExtensions
    {
        public static ContactForm ToContactForm(this ContactFormModel contactFormModel)
        {
            return new ContactForm
            {
                Id = Guid.NewGuid(),
                Name = contactFormModel.Name,
                Email = contactFormModel.Email,
                Message = contactFormModel.Message,
                DateCreatedUtc = DateTime.UtcNow
            };
        }
    }
}
