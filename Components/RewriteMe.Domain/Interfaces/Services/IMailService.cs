using System.Threading.Tasks;

namespace RewriteMe.Domain.Interfaces.Services
{
    public interface IMailService
    {
        Task SendAsync(string recipient, string subject, string body);
    }
}
