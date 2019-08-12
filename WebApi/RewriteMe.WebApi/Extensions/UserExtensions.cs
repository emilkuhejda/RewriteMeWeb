using RewriteMe.Domain.UserManagement;
using RewriteMe.WebApi.Dtos;

namespace RewriteMe.WebApi.Extensions
{
    public static class UserExtensions
    {
        public static UserIdentityDto ToIdentityDto(this User user)
        {
            return new UserIdentityDto
            {
                Id = user.Id,
                Email = user.Email,
                GivenName = user.GivenName,
                FamilyName = user.FamilyName
            };
        }
    }
}
