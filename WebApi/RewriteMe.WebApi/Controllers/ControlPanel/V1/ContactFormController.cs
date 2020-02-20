using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RewriteMe.Domain.Enums;
using RewriteMe.Domain.Interfaces.Services;

namespace RewriteMe.WebApi.Controllers.ControlPanel.V1
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/control-panel/contact-forms")]
    [Produces("application/json")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize(Roles = nameof(Role.Admin))]
    [ApiController]
    public class ContactFormController : ControllerBase
    {
        private readonly IContactFormService _contactFormService;

        public ContactFormController(IContactFormService contactFormService)
        {
            _contactFormService = contactFormService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var contactForms = await _contactFormService.GetAllAsync().ConfigureAwait(false);

            return Ok(contactForms);
        }

        [HttpGet("{contactFormId}")]
        public async Task<IActionResult> Get(Guid contactFormId)
        {
            var contactForm = await _contactFormService.GetAsync(contactFormId).ConfigureAwait(false);

            return Ok(contactForm);
        }
    }
}