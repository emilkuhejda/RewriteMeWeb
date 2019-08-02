using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RewriteMe.Domain.Forms;

namespace RewriteMe.Domain.Interfaces.Repositories
{
    public interface IContactFormRepository
    {
        Task AddAsync(ContactForm contactForm);

        Task<ContactForm> GetAsync(Guid contactFormId);

        Task<IEnumerable<ContactForm>> GetAllAsync();
    }
}
