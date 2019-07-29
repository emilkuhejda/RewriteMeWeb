using RewriteMe.Common.Helpers;
using RewriteMe.Domain.Administration;
using RewriteMe.WebApi.Models;

namespace RewriteMe.WebApi.Extensions
{
    public static class UpdateAdministratorModelExtensions
    {
        public static Administrator ToAdministrator(this UpdateAdministratorModel model)
        {
            var administrator = new Administrator
            {
                Id = model.Id,
                FirstName = model.FirstName,
                LastName = model.LastName
            };

            if (!string.IsNullOrWhiteSpace(model.Password))
            {
                var password = PasswordHelper.CreateHash(model.Password);
                administrator.PasswordHash = password.PasswordHash;
                administrator.PasswordSalt = password.PasswordSalt;
            }

            return administrator;
        }
    }
}
