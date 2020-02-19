using RewriteMe.Domain.Dtos;
using RewriteMe.Domain.Settings;
using RewriteMe.WebApi.Models;

namespace RewriteMe.WebApi.Commands
{
    public class RegisterUserCommand : CommandBase<UserRegistrationDto>
    {
        public RegistrationUserModel RegistrationUserModel { get; set; }

        public AppSettings AppSettings { get; set; }
    }
}
