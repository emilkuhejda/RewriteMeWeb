using System;
using RewriteMe.Domain.UserManagement;
using RewriteMe.WebApi.Models;

namespace RewriteMe.WebApi.Extensions
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
                DateRegistered = DateTime.UtcNow
            };
        }
    }
}
