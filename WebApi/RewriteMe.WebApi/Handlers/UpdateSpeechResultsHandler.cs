using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using RewriteMe.Domain.Dtos;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.WebApi.Commands;

namespace RewriteMe.WebApi.Handlers
{
    public class UpdateSpeechResultsHandler : IRequestHandler<UpdateSpeechResultsCommand, TimeSpanWrapperDto>
    {
        private readonly IUserSubscriptionService _userSubscriptionService;
        private readonly ISpeechResultService _speechResultService;
        private readonly IApplicationLogService _applicationLogService;

        public UpdateSpeechResultsHandler(
            IUserSubscriptionService userSubscriptionService,
            ISpeechResultService speechResultService,
            IApplicationLogService applicationLogService)
        {
            _userSubscriptionService = userSubscriptionService;
            _speechResultService = speechResultService;
            _applicationLogService = applicationLogService;
        }

        public async Task<TimeSpanWrapperDto> Handle(UpdateSpeechResultsCommand request, CancellationToken cancellationToken)
        {
            await _speechResultService.UpdateAllAsync(request.SpeechResults).ConfigureAwait(false);

            var totalTimeTicks = request.SpeechResults.Sum(x => x.TotalTime.Ticks);
            var totalTime = TimeSpan.FromTicks(totalTimeTicks);
            await _userSubscriptionService.SubtractTimeAsync(request.UserId, totalTime).ConfigureAwait(false);

            await _applicationLogService.InfoAsync("Update speech results total time.", request.UserId).ConfigureAwait(false);

            var remainingTime = await _userSubscriptionService.GetRemainingTimeAsync(request.UserId).ConfigureAwait(false);
            var timeSpanWrapperDto = new TimeSpanWrapperDto { Ticks = remainingTime.Ticks };

            return timeSpanWrapperDto;
        }
    }
}
