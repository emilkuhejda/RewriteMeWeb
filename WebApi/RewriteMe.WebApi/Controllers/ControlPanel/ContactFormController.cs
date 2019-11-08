using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RewriteMe.Common.Utils;
using RewriteMe.Domain.Enums;
using RewriteMe.Domain.Interfaces.Services;

namespace RewriteMe.WebApi.Controllers.ControlPanel
{
    [Produces("application/json")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize(Roles = nameof(Role.Admin))]
    [ApiController]
    public class ContactFormController : ControllerBase
    {
        private readonly IContactFormService _contactFormService;
        private readonly IApplicationLogService _applicationLogService;

        public ContactFormController(
            IContactFormService contactFormService,
            IApplicationLogService applicationLogService)
        {
            _contactFormService = contactFormService;
            _applicationLogService = applicationLogService;
        }

        [HttpGet("/api/control-panel/contact-forms")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var contactForms = await _contactFormService.GetAllAsync().ConfigureAwait(false);

                return Ok(contactForms);
            }
            catch (Exception ex)
            {
                await _applicationLogService.ErrorAsync($"{ExceptionFormatter.FormatException(ex)}").ConfigureAwait(false);
            }

            return StatusCode((int)HttpStatusCode.InternalServerError);
        }

        [HttpGet("/api/control-panel/contact-forms/{contactFormId}")]
        public async Task<IActionResult> Get(Guid contactFormId)
        {
            try
            {
                var contactForm = await _contactFormService.GetAsync(contactFormId).ConfigureAwait(false);

                return Ok(contactForm);
            }
            catch (Exception ex)
            {
                await _applicationLogService.ErrorAsync($"{ExceptionFormatter.FormatException(ex)}").ConfigureAwait(false);
            }

            return StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }
}