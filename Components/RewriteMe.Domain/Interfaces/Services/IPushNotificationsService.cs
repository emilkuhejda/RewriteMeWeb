using System.Threading.Tasks;
using RewriteMe.Domain.Messages;
using RewriteMe.Domain.Notifications;

namespace RewriteMe.Domain.Interfaces.Services
{
    public interface IPushNotificationsService
    {
        Task<NotificationResult> SendAsync(InformationMessage informationMessage);
    }
}
