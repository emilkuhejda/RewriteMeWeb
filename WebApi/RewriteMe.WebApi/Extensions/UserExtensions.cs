using RewriteMe.Domain.UserManagement;
using RewriteMe.WebApi.Dtos;

namespace RewriteMe.WebApi.Extensions
{
    public static class UserExtensions
    {
        public static IdentityDto ToIdentityDto(this User user)
        {
            return new IdentityDto
            {
                Id = user.Id,
                Email = user.Email,
                GivenName = user.GivenName,
                FamilyName = user.FamilyName
            };
        }
    }
}
