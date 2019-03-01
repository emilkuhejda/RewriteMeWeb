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
                Id = Guid.NewGuid(),
                Username = registerUserModel.Username,
                FirstName = registerUserModel.FirstName,
                LastName = registerUserModel.LastName
            };
        }
    }
}
