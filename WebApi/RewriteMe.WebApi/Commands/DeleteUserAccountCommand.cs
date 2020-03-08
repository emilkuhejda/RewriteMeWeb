using RewriteMe.Domain.Dtos;

namespace RewriteMe.WebApi.Commands
{
    public class DeleteUserAccountCommand : CommandBase<OkDto>
    {
        public string Email { get; set; }
    }
}
