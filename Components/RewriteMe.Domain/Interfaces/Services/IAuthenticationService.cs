using RewriteMe.Domain.UserManagement;

namespace RewriteMe.Domain.Interfaces.Services
{
    public interface IAuthenticationService
    {
        void CalculatePasswordHash(User user, string password);

        User Authenticate(string username, string password);
    }
}
