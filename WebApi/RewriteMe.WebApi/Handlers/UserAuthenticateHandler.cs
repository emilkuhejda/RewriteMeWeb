using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using RewriteMe.Domain.Dtos;
using RewriteMe.Domain.Enums;
using RewriteMe.Domain.Exceptions;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.WebApi.Commands;
using RewriteMe.WebApi.Extensions;
using RewriteMe.WebApi.Utils;
using Serilog;

namespace RewriteMe.WebApi.Handlers
{
    public class UserAuthenticateHandler : IRequestHandler<UserAuthenticateCommand, AdministratorDto>
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly ILogger _logger;

        public UserAuthenticateHandler(
            IAuthenticationService authenticationService,
            ILogger logger)
        {
            _authenticationService = authenticationService;
            _logger = logger.ForContext<UserAuthenticateHandler>();
        }

        public async Task<AdministratorDto> Handle(UserAuthenticateCommand request, CancellationToken cancellationToken)
        {
            _logger.Information($"Administrator authentication with user name '{request.Username}'.");

            var administrator = await _authenticationService.AuthenticateAsync(request.Username, request.Password).ConfigureAwait(false);
            if (administrator == null)
            {
                _logger.Error($"Administrator '{request.Username}' was not found.");

                throw new OperationErrorException(StatusCodes.Status404NotFound);
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, administrator.Id.ToString()),
                new Claim(ClaimTypes.Role, Role.Admin.ToString())
            };

            var token = TokenHelper.Generate(request.AppSettings.SecretKey, claims, TimeSpan.FromDays(7));

            _logger.Information($"Administrator '{request.Username}' was successfully authenticated.");

            return administrator.ToDto(token);
        }
    }
}
