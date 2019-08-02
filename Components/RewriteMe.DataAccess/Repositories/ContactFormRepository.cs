using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RewriteMe.DataAccess.DataAdapters;
using RewriteMe.Domain.Forms;
using RewriteMe.Domain.Interfaces.Repositories;

namespace RewriteMe.DataAccess.Repositories
{
    public class ContactFormRepository : IContactFormRepository
    {
        private readonly IDbContextFactory _contextFactory;

        public ContactFormRepository(IDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task AddAsync(ContactForm contactForm)
        {
            using (var context = _contextFactory.Create())
            {
                await context.ContactForms.AddAsync(contactForm.ToContactFormEntity()).ConfigureAwait(false);
                await context.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        public async Task<IEnumerable<ContactForm>> GetAllAsync()
        {
            using (var context = _contextFactory.Create())
            {
                return await context.ContactForms.AsNoTracking().Select(x => x.ToContactForm()).ToListAsync().ConfigureAwait(false);
            }
        }
    }
}
