using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using RewriteMe.Business.Polling;
using RewriteMe.Domain.Interfaces.Services;

namespace RewriteMe.Business.Services
{
    public class MessageCenterService : IMessageCenterService
    {
        private readonly IHubContext<MessageHub> _messageHub;

        public MessageCenterService(IHubContext<MessageHub> messageHub)
        {
            _messageHub = messageHub;
        }

        public async Task SendAsync(string method, object arg1)
        {
            await _messageHub.Clients.All.SendAsync(method, arg1).ConfigureAwait(false);
        }

        public async Task SendAsync(string method, object arg1, object arg2)
        {
            await _messageHub.Clients.All.SendAsync(method, arg1, arg2).ConfigureAwait(false);
        }
    }
}
