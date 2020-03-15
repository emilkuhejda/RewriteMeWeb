using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Newtonsoft.Json;
using RewriteMe.Business.Utils;
using RewriteMe.Domain.Dtos;
using RewriteMe.Domain.Enums;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.WebApi.Commands;
using RewriteMe.WebApi.Extensions;
using RewriteMe.WebApi.Utils;
using Serilog;

namespace RewriteMe.WebApi.Handlers
{
    public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, UserRegistrationDto>
    {
        private readonly IUserService _userService;
        private readonly IUserSubscriptionService _userSubscriptionService;
        private readonly IUserDeviceService _userDeviceService;
        private readonly ILogger _logger;

        public RegisterUserHandler(
            IUserService userService,
            IUserSubscriptionService userSubscriptionService,
            IUserDeviceService userDeviceService,
            ILogger logger)
        {
            _userService = userService;
            _userSubscriptionService = userSubscriptionService;
            _userDeviceService = userDeviceService;
            _logger = logger;
        }

        public async Task<UserRegistrationDto> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            _logger.Information($"Attempt to register user with ID = '{request.RegistrationUserModel.Id}'.");

            TimeSpan remainingTime;
            var user = await _userService.GetAsync(request.RegistrationUserModel.Id).ConfigureAwait(false);
            if (user == null)
            {
                user = request.RegistrationUserModel.ToUser();
                await _userService.AddAsync(user).ConfigureAwait(false);
                _logger.Information($"User with ID = '{user.Id}' and Email = '{user.Email}' was created.");

                var userSubscription = SubscriptionHelper.CreateFreeSubscription(user.Id, request.RegistrationUserModel.ApplicationId);
                await _userSubscriptionService.AddAsync(userSubscription).ConfigureAwait(false);
                _logger.Information($"Basic {userSubscription.Time.TotalMinutes} minutes subscription with ID = '{userSubscription.Id}' was created. [{user.Id}]");

                remainingTime = userSubscription.Time;
            }
            else
            {
                remainingTime = await _userSubscriptionService.GetRemainingTimeAsync(user.Id).ConfigureAwait(false);
            }

            if (request.RegistrationUserModel.Device != null)
            {
                var userDevice = request.RegistrationUserModel.Device.ToUserDevice(user.Id);
                await _userDeviceService.AddOrUpdateAsync(userDevice).ConfigureAwait(false);
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, Role.User.ToString())
            };
            var token = TokenHelper.Generate(request.AppSettings.SecretKey, claims, TimeSpan.FromDays(180));

            var refreshClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, Role.Security.ToString())
            };
            var refreshToken = TokenHelper.Generate(request.AppSettings.SecretKey, refreshClaims, TimeSpan.FromDays(730));

            var registrationModelDto = new UserRegistrationDto
            {
                Token = token,
                RefreshToken = refreshToken,
                Identity = user.ToIdentityDto(),
                RemainingTime = new TimeSpanWrapperDto { Ticks = remainingTime.Ticks }
            };

            _logger.Information($"User was successfully registered. User: {JsonConvert.SerializeObject(registrationModelDto.Identity)}");

            return registrationModelDto;
        }
    }
}
