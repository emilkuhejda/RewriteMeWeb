using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using RewriteMe.Business.Services;
using RewriteMe.DataAccess;
using RewriteMe.DataAccess.Repositories;
using RewriteMe.Domain.Interfaces.Repositories;
using RewriteMe.Domain.Interfaces.Services;

namespace RewriteMe.WebApi.Services
{
    public class Bootstrapper
    {
        public static AutofacServiceProvider BootstrapRuntime(IServiceCollection services)
        {
            var builder = new ContainerBuilder();
            builder.Populate(services);

            BindCommon(builder);
            var container = builder.Build();

            return new AutofacServiceProvider(container);
        }

        private static void BindCommon(ContainerBuilder builder)
        {
            builder.RegisterType<DbContextFactory>().As<IDbContextFactory>().InstancePerLifetimeScope();
            builder.RegisterType<UserRepository>().As<IUserRepository>().InstancePerLifetimeScope();

            builder.RegisterType<AuthenticationService>().As<IAuthenticationService>().InstancePerLifetimeScope();
            builder.RegisterType<UserService>().As<IUserService>().InstancePerLifetimeScope();
        }
    }
}
