using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.WebApi.Dtos;
using RewriteMe.WebApi.Extensions;
using RewriteMe.WebApi.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace RewriteMe.WebApi.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [ApiController]
    public class ContactFormController : ControllerBase
    {
        private readonly IContactFormService _contactFormService;

        public ContactFormController(IContactFormService contactFormService)
        {
            _contactFormService = contactFormService;
        }

        [AllowAnonymous]
        [HttpPost("/api/contact-form/create")]
        [ProducesResponseType(typeof(OkDto), StatusCodes.Status200OK)]
        [SwaggerOperation(OperationId = "CreateContactForm")]
        public async Task<IActionResult> Create([FromBody] ContactFormModel contactFormModel)
        {
            var contactForm = contactFormModel.ToContactForm();

            await _contactFormService.AddAsync(contactForm).ConfigureAwait(false);

            return Ok(new OkDto());
        }
    }
}