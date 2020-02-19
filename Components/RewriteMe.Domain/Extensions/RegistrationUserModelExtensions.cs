using System;
using RewriteMe.Domain.UserManagement;

namespace RewriteMe.Domain.Extensions
{
    public static class RegistrationUserModelExtensions
    {
        public static User ToUser(this RegistrationUserModel registrationUserModel)
        {
            return new User
            {
                Id = registrationUserModel.Id,
                Email = registrationUserModel.Email,
                GivenName = registrationUserModel.GivenName,
                FamilyName = registrationUserModel.FamilyName,
                DateRegisteredUtc = DateTime.UtcNow
            };
        }
    }
}
