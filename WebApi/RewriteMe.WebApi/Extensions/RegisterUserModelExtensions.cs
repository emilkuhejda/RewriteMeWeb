using System;
using RewriteMe.Domain.UserManagement;
using RewriteMe.WebApi.Models;

namespace RewriteMe.WebApi.Extensions
{
    public static class RegisterUserModelExtensions
    {
        public static User ToUser(this RegisterUserModel registerUserModel)
        {
            return new User
            {
                Id = registerUserModel.Id,
                Email = registerUserModel.Email,
                GivenName = registerUserModel.GivenName,
                FamilyName = registerUserModel.FamilyName,
                ApplicationId = registerUserModel.ApplicationId,
                DateRegistered = DateTime.UtcNow
            };
        }
    }
}
