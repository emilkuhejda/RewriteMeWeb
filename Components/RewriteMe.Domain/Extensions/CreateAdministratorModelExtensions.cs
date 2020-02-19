using System;
using RewriteMe.Common.Helpers;
using RewriteMe.Domain.Administration;

namespace RewriteMe.Domain.Extensions
{
    public static class CreateAdministratorModelExtensions
    {
        public static Administrator ToAdministrator(this CreateAdministratorModel model)
        {
            var password = PasswordHelper.CreateHash(model.Password);

            return new Administrator
            {
                Id = Guid.NewGuid(),
                Username = model.Username,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PasswordHash = password.PasswordHash,
                PasswordSalt = password.PasswordSalt
            };
        }
    }
}
