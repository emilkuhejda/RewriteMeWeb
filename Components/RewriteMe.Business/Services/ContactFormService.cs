using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RewriteMe.Domain.Forms;
using RewriteMe.Domain.Interfaces.Repositories;
using RewriteMe.Domain.Interfaces.Services;
using Serilog;

namespace RewriteMe.Business.Services
{
    public class ContactFormService : IContactFormService
    {
        private readonly IContactFormRepository _contactFormRepository;
        private readonly ILogger _logger;

        public ContactFormService(
            IContactFormRepository contactFormRepository,
            ILogger logger)
        {
            _contactFormRepository = contactFormRepository;
            _logger = logger.ForContext<ContactFormService>();
        }

        public async Task AddAsync(ContactForm contactForm)
        {
            await _contactFormRepository.AddAsync(contactForm).ConfigureAwait(false);

            _logger.Information($"New contact '{contactForm.Id}' form was created.");
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
