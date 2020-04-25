using System.Threading.Tasks;

namespace RewriteMe.Domain.Interfaces.Services
{
    public interface IMessageCenterService
    {
        Task SendAsync(string method, object arg1);

        Task SendAsync(string method, object arg1, object arg2);
    }
}
