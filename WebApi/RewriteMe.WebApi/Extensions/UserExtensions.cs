using RewriteMe.Domain.UserManagement;
using RewriteMe.WebApi.Dtos;

namespace RewriteMe.WebApi.Extensions
{
    public static class UserExtensions
    {
        public static UserDto ToDto(this User user)
        {
            return new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                GivenName = user.GivenName,
                FamilyName = user.FamilyName
            };
        }
    }
}
