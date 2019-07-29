using System;
using RewriteMe.Common.Helpers;
using RewriteMe.Domain.Administration;
using RewriteMe.WebApi.Models;

namespace RewriteMe.WebApi.Extensions
{
    public static class AdministratorModelExtensions
    {
        public static Administrator ToAdministrator(this AdministratorModel model)
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
