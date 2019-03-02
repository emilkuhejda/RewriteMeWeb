using System.Threading.Tasks;
using RewriteMe.Domain.UserManagement;

namespace RewriteMe.Domain.Interfaces.Services
{
    public interface IAuthenticationService
    {
        void CalculatePasswordHash(User user, string password);

        Task<User> AuthenticateAsync(string username, string password);
    }
}
