using System.Collections.Generic;
using System.Threading.Tasks;
using RewriteMe.Domain.Forms;

namespace RewriteMe.Domain.Interfaces.Services
{
    public interface IContactFormService
    {
        Task AddAsync(ContactForm contactForm);

        Task<IEnumerable<ContactForm>> GetAllAsync();
    }
}
