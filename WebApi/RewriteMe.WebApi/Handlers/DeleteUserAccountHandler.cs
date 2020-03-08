using System;
using System.Threading;
using System.Threading.Tasks;
using Hangfire;
using MediatR;
using Microsoft.AspNetCore.Http;
using RewriteMe.Domain.Dtos;
using RewriteMe.Domain.Exceptions;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.Domain.UserManagement;
using RewriteMe.WebApi.Commands;

namespace RewriteMe.WebApi.Handlers
{
    public class DeleteUserAccountHandler : IRequestHandler<DeleteUserAccountCommand, OkDto>
    {
        private readonly IUserService _userService;
        private readonly IDeletedAccountService _deletedAccountService;

        public DeleteUserAccountHandler(
            IUserService userService,
            IDeletedAccountService deletedAccountService)
        {
            _userService = userService;
            _deletedAccountService = deletedAccountService;
        }

        public async Task<OkDto> Handle(DeleteUserAccountCommand request, CancellationToken cancellationToken)
        {
            var userExists = await _userService.ExistsAsync(request.UserId, request.Email).ConfigureAwait(false);
            if (!userExists)
                throw new OperationErrorException(StatusCodes.Status404NotFound);

            var deletedAccount = new DeletedAccount
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                DateDeleted = DateTime.UtcNow
            };

            await _deletedAccountService.AddAsync(deletedAccount).ConfigureAwait(false);

            BackgroundJob.Enqueue(() => _userService.DeleteAsync(request.UserId));

            return new OkDto();
        }
    }
}
