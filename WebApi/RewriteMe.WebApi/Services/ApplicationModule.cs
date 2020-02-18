using Autofac;
using MediatR;
using RewriteMe.Business;
using RewriteMe.DataAccess;
using RewriteMe.Domain.Interfaces.Services;

namespace RewriteMe.WebApi.Services
{
    public class ApplicationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            RegisterModules(builder);
            RegisterServices(builder);
        }

        private void RegisterModules(ContainerBuilder builder)
        {
            builder.RegisterModule<BusinessModule>();
            builder.RegisterModule<DataAccessModule>();
        }

        public static void RegisterServices(ContainerBuilder builder)
        {
            builder.RegisterType<FileAccessService>().As<IFileAccessService>().InstancePerLifetimeScope();
            builder.RegisterType<Mediator>().As<IMediator>().InstancePerLifetimeScope();
            builder.Register<ServiceFactory>(context =>
            {
                var ctx = context.Resolve<IComponentContext>();
                return t => ctx.Resolve(t);
            });
        }
    }
}
