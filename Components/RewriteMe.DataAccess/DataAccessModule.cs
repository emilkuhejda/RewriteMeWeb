using Autofac;
using RewriteMe.DataAccess.Repositories;
using RewriteMe.Domain.Interfaces.Repositories;

namespace RewriteMe.DataAccess
{
    public class DataAccessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            RegisterServices(builder);
        }

        public static void RegisterServices(ContainerBuilder builder)
        {
            builder.RegisterType<DbContextFactory>().As<IDbContextFactory>().InstancePerLifetimeScope();
            builder.RegisterType<AdministratorRepository>().As<IAdministratorRepository>().InstancePerLifetimeScope();
            builder.RegisterType<UserRepository>().As<IUserRepository>().InstancePerLifetimeScope();
            builder.RegisterType<FileItemRepository>().As<IFileItemRepository>().InstancePerLifetimeScope();
            builder.RegisterType<FileItemSourceRepository>().As<IFileItemSourceRepository>().InstancePerLifetimeScope();
            builder.RegisterType<UploadedChunkRepository>().As<IUploadedChunkRepository>().InstancePerLifetimeScope();
            builder.RegisterType<TranscribeItemRepository>().As<ITranscribeItemRepository>().InstancePerLifetimeScope();
            builder.RegisterType<TranscribeItemSourceRepository>().As<ITranscribeItemSourceRepository>().InstancePerLifetimeScope();
            builder.RegisterType<RecognizedAudioSampleRepository>().As<IRecognizedAudioSampleRepository>().InstancePerLifetimeScope();
            builder.RegisterType<SpeechResultRepository>().As<ISpeechResultRepository>().InstancePerLifetimeScope();
            builder.RegisterType<UserSubscriptionRepository>().As<IUserSubscriptionRepository>().InstancePerLifetimeScope();
            builder.RegisterType<BillingPurchaseRepository>().As<IBillingPurchaseRepository>().InstancePerLifetimeScope();
            builder.RegisterType<ContactFormRepository>().As<IContactFormRepository>().InstancePerLifetimeScope();
            builder.RegisterType<InformationMessageRepository>().As<IInformationMessageRepository>().InstancePerLifetimeScope();
            builder.RegisterType<LanguageVersionRepository>().As<ILanguageVersionRepository>().InstancePerLifetimeScope();
            builder.RegisterType<UserDeviceRepository>().As<IUserDeviceRepository>().InstancePerLifetimeScope();
            builder.RegisterType<DeletedAccountRepository>().As<IDeletedAccountRepository>().InstancePerLifetimeScope();
            builder.RegisterType<InternalValueRepository>().As<IInternalValueRepository>().InstancePerLifetimeScope();
            builder.RegisterType<DatabaseRepository>().As<IDatabaseRepository>().InstancePerLifetimeScope();
        }
    }
}
