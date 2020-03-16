using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RewriteMe.Domain.Dtos;
using RewriteMe.Domain.Enums;
using RewriteMe.Domain.Settings;
using RewriteMe.WebApi.Commands;
using RewriteMe.WebApi.Models;
using Serilog;

namespace RewriteMe.WebApi.Controllers.V1
{
    [ApiVersion("1")]
    [Produces("application/json")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly AppSettings _appSettings;
        private readonly ILogger _logger;

        public AuthenticationController(
            IMediator mediator,
            IOptions<AppSettings> options,
            ILogger logger)
        {
            _mediator = mediator;
            _appSettings = options.Value;
            _logger = logger.ForContext<AuthenticationController>();
        }

        [AllowAnonymous]
        [HttpPost("/api/v{version:apiVersion}/authenticate")]
        [ProducesResponseType(typeof(AdministratorDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorCode), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Authenticate([FromBody]AuthenticationModel authenticationModel)
        {
            _logger.Information($"User authentication with username = '{authenticationModel.Username}'.");

            var userAuthenticateCommand = new UserAuthenticateCommand
            {
                Username = authenticationModel.Username,
                Password = authenticationModel.Password,
                AppSettings = _appSettings
            };

            var administratorDto = await _mediator.Send(userAuthenticateCommand).ConfigureAwait(false);

            _logger.Information($"User authentication for '{authenticationModel.Username}' was successful.");

            return Ok(administratorDto);
        }
    }
}