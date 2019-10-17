using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using RewriteMe.Domain.Administration;
using RewriteMe.Domain.Interfaces.Services;

namespace RewriteMe.Business.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IAdministratorService _administratorService;

        public AuthenticationService(IAdministratorService administratorService)
        {
            _administratorService = administratorService;
        }

        public async Task<Administrator> AuthenticateAsync(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                return null;

            var administrator = await _administratorService.GetAsync(username).ConfigureAwait(false);
            if (administrator == null)
                return null;

            if (!VerifyPasswordHash(password, administrator.PasswordHash, administrator.PasswordSalt))
                return null;

            return administrator;
        }

        private bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null)
                throw new ArgumentNullException(nameof(password));

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Value cannot be empty or whitespace only string.", nameof(password));

            if (storedHash.Length != 64)
                throw new ArgumentException("Invalid length of password hash (64 bytes expected).", nameof(storedHash));

            if (storedSalt.Length != 128)
                throw new ArgumentException("nvalid length of password salt (128 bytes expected).", nameof(storedSalt));

            using (var hmac = new HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                for (var i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i])
                        return false;
                }
            }

            return true;
        }

        public string GenerateHash(string password)
        {
            using (var sha256Managed = new SHA256Managed())
            {
                var computedHash = sha256Managed.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(computedHash).Replace("-", string.Empty, StringComparison.InvariantCulture);
            }
        }
    }
}
