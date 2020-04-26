using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RewriteMe.Domain.Enums;
using RewriteMe.WebApi.Commands;
using RewriteMe.WebApi.Extensions;
using RewriteMe.WebApi.Models;

namespace RewriteMe.WebApi.Controllers.V1
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/mail")]
    [Produces("application/json")]
    [Authorize(Roles = nameof(Role.User))]
    [ApiExplorerSettings(IgnoreApi = true)]
    [ApiController]
    public class MailController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MailController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> SendEmail([FromBody]MailModel mailModel)
        {
            var userId = HttpContext.User.GetNameIdentifier();
            var sendMailCommand = new SendMailCommand
            {
                UserId = userId,
                FileItemId = mailModel.FileItemId,
                Recipient = mailModel.Recipient
            };

            var okDto = await _mediator.Send(sendMailCommand).ConfigureAwait(false);

            return Ok(okDto);
        }
    }
}