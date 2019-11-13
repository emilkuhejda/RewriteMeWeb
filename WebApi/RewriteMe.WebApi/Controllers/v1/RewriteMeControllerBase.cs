using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.Domain.UserManagement;
using RewriteMe.WebApi.Extensions;

namespace RewriteMe.WebApi.Controllers.v1
{
    public class RewriteMeControllerBase : ControllerBase
    {
        public RewriteMeControllerBase(IUserService userService)
        {
            UserService = userService;
        }

        protected IUserService UserService { get; }

        protected async Task<User> VerifyUserAsync()
        {
            var userId = HttpContext.User.GetNameIdentifier();
            var user = await UserService.GetAsync(userId).ConfigureAwait(false);
            return user;
        }
    }
}
