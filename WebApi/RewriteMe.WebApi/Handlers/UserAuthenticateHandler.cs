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

namespace RewriteMe.WebApi.Handlers
{
    public class UserAuthenticateHandler : IRequestHandler<UserAuthenticateCommand, AdministratorDto>
    {
        private readonly IAuthenticationService _authenticationService;

        public UserAuthenticateHandler(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        public async Task<AdministratorDto> Handle(UserAuthenticateCommand request, CancellationToken cancellationToken)
        {
            var administrator = await _authenticationService.AuthenticateAsync(request.Username, request.Password).ConfigureAwait(false);
            if (administrator == null)
                throw new OperationErrorException(StatusCodes.Status404NotFound);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, administrator.Id.ToString()),
                new Claim(ClaimTypes.Role, Role.Admin.ToString())
            };

            var token = TokenHelper.Generate(request.AppSettings.SecretKey, claims, TimeSpan.FromDays(7));

            return administrator.ToDto(token);
        }
    }
}
