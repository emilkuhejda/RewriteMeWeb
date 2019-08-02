using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RewriteMe.Domain.Forms;
using RewriteMe.Domain.Interfaces.Repositories;
using RewriteMe.Domain.Interfaces.Services;

namespace RewriteMe.Business.Services
{
    public class ContactFormService : IContactFormService
    {
        private readonly IContactFormRepository _contactFormRepository;

        public ContactFormService(IContactFormRepository contactFormRepository)
        {
            _contactFormRepository = contactFormRepository;
        }

        public async Task AddAsync(ContactForm contactForm)
        {
            await _contactFormRepository.AddAsync(contactForm).ConfigureAwait(false);
        }

        public async Task<ContactForm> GetAsync(Guid contactFormId)
        {
            return await _contactFormRepository.GetAsync(contactFormId).ConfigureAwait(false);
        }

        public async Task<IEnumerable<ContactForm>> GetAllAsync()
        {
            return await _contactFormRepository.GetAllAsync().ConfigureAwait(false);
        }
    }
}
