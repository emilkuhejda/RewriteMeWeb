using RewriteMe.Domain.Dtos;
using RewriteMe.Domain.Settings;

namespace RewriteMe.WebApi.Commands
{
    public class UserAuthenticateCommand : CommandBase<AdministratorDto>
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public AppSettings AppSettings { get; set; }
    }
}
