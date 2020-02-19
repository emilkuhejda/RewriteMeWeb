using RewriteMe.Domain.Administration;
using RewriteMe.Domain.Dtos;

namespace RewriteMe.Domain.Extensions
{
    public static class AdministratorExtensions
    {
        public static AdministratorDto ToDto(this Administrator administrator, string token)
        {
            return new AdministratorDto
            {
                Id = administrator.Id,
                Username = administrator.Username,
                FirstName = administrator.FirstName,
                LastName = administrator.LastName,
                Token = token
            };
        }
    }
}
