﻿using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RewriteMe.Common.Utils;
using RewriteMe.Domain.Dtos;
using RewriteMe.Domain.Enums;
using RewriteMe.Domain.Exceptions;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.WebApi.Commands;

namespace RewriteMe.WebApi.Handlers
{
    public class CreateUserSubscriptionHandler : IRequestHandler<CreateUserSubscriptionCommand, TimeSpanWrapperDto>
    {
        private readonly IUserSubscriptionService _userSubscriptionService;
        private readonly IApplicationLogService _applicationLogService;

        public CreateUserSubscriptionHandler(
            IUserSubscriptionService userSubscriptionService,
            IApplicationLogService applicationLogService)
        {
            _userSubscriptionService = userSubscriptionService;
            _applicationLogService = applicationLogService;
        }

        public async Task<TimeSpanWrapperDto> Handle(CreateUserSubscriptionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.UserId != request.BillingPurchase.UserId)
                    throw new OperationErrorException(ErrorCode.EC301);

                var userSubscription = await _userSubscriptionService.RegisterPurchaseAsync(request.BillingPurchase, request.ApplicationId).ConfigureAwait(false);
                if (userSubscription == null)
                    throw new OperationErrorException(ErrorCode.EC302);

                var remainingTime = await _userSubscriptionService.GetRemainingTimeAsync(request.UserId).ConfigureAwait(false);
                var timeSpanWrapperDto = new TimeSpanWrapperDto { Ticks = remainingTime.Ticks };

                return timeSpanWrapperDto;
            }
            catch (DbUpdateException ex)
            {
                await _applicationLogService.ErrorAsync($"{ExceptionFormatter.FormatException(ex)}").ConfigureAwait(false);

                throw new OperationErrorException(ErrorCode.EC400);
            }
        }
    }
}