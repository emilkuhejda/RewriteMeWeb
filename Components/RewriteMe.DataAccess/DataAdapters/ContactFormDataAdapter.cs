using RewriteMe.DataAccess.Entities;
using RewriteMe.Domain.Forms;

namespace RewriteMe.DataAccess.DataAdapters
{
    public static class ContactFormDataAdapter
    {
        public static ContactForm ToContactForm(this ContactFormEntity entity)
        {
            return new ContactForm
            {
                Id = entity.Id,
                Name = entity.Name,
                Email = entity.Email,
                Message = entity.Message,
                DateCreated = entity.DateCreated
            };
        }

        public static ContactFormEntity ToContactFormEntity(this ContactForm contactForm)
        {
            return new ContactFormEntity
            {
                Id = contactForm.Id,
                Name = contactForm.Name,
                Email = contactForm.Email,
                Message = contactForm.Message,
                DateCreated = contactForm.DateCreated
            };
        }
    }
}
