using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RewriteMe.Domain.Dtos;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.WebApi.Extensions;
using RewriteMe.WebApi.Models;
using Serilog;
using Swashbuckle.AspNetCore.Annotations;

namespace RewriteMe.WebApi.Controllers.V1
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/contact-form")]
    [Produces("application/json")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [ApiController]
    public class ContactFormController : ControllerBase
    {
        private readonly IContactFormService _contactFormService;
        private readonly ILogger _logger;

        public ContactFormController(
            IContactFormService contactFormService,
            ILogger logger)
        {
            _contactFormService = contactFormService;
            _logger = logger.ForContext<ContactFormController>();
        }

        [AllowAnonymous]
        [HttpPost("create")]
        [ProducesResponseType(typeof(OkDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(OperationId = "CreateContactForm")]
        public async Task<IActionResult> Create([FromBody]ContactFormModel contactFormModel)
        {
            var contactForm = contactFormModel.ToContactForm();

            await _contactFormService.AddAsync(contactForm).ConfigureAwait(false);

            _logger.Information($"Contact '{contactForm.Id}' form was created.");

            return Ok(new OkDto());
        }
    }
}