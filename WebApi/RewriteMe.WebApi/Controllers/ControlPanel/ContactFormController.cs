using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RewriteMe.Domain.Interfaces.Services;

namespace RewriteMe.WebApi.Controllers.ControlPanel
{
    [Route("control-panel/[controller]")]
    [Produces("application/json")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize]
    [ApiController]
    public class ContactFormController : ControllerBase
    {
        private readonly IContactFormService _contactFormService;

        public ContactFormController(IContactFormService contactFormService)
        {
            _contactFormService = contactFormService;
        }

        [HttpGet("/control-panel/contact-form")]
        public async Task<IActionResult> GetAll()
        {
            var contactForms = await _contactFormService.GetAllAsync().ConfigureAwait(false);

            return Ok(contactForms);
        }

        [HttpGet("/control-panel/contact-form/{contactFormId}")]
        public async Task<IActionResult> Get(Guid contactFormId)
        {
            var contactForm = await _contactFormService.GetAsync(contactFormId).ConfigureAwait(false);

            return Ok(contactForm);
        }
    }
}