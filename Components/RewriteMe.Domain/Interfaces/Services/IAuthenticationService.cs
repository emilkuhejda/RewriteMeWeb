using System.Threading.Tasks;
using RewriteMe.Domain.Administration;

namespace RewriteMe.Domain.Interfaces.Services
{
    public interface IAuthenticationService
    {
        Task<Administrator> AuthenticateAsync(string username, string password);

        string GenerateHash(string password);
    }
}
