using Autofac;
using RewriteMe.Business.Managers;
using RewriteMe.Business.Services;
using RewriteMe.Domain.Interfaces.Managers;
using RewriteMe.Domain.Interfaces.Services;
using Module = Autofac.Module;

namespace RewriteMe.Business
{
    public class BusinessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            RegisterServices(builder);
        }

        public static void RegisterServices(ContainerBuilder builder)
        {
            builder.RegisterType<AdministratorService>().As<IAdministratorService>().InstancePerLifetimeScope();
            builder.RegisterType<UserService>().As<IUserService>().InstancePerLifetimeScope();
            builder.RegisterType<FileItemService>().As<IFileItemService>().InstancePerLifetimeScope();
            builder.RegisterType<FileItemSourceService>().As<IFileItemSourceService>().InstancePerLifetimeScope();
            builder.RegisterType<UploadedChunkService>().As<IUploadedChunkService>().InstancePerLifetimeScope();
            builder.RegisterType<TranscribeItemService>().As<ITranscribeItemService>().InstancePerLifetimeScope();
            builder.RegisterType<TranscribeItemSourceService>().As<ITranscribeItemSourceService>().InstancePerLifetimeScope();
            builder.RegisterType<RecognizedAudioSampleService>().As<IRecognizedAudioSampleService>().InstancePerLifetimeScope();
            builder.RegisterType<SpeechResultService>().As<ISpeechResultService>().InstancePerLifetimeScope();
            builder.RegisterType<UserSubscriptionService>().As<IUserSubscriptionService>().InstancePerLifetimeScope();
            builder.RegisterType<ContactFormService>().As<IContactFormService>().InstancePerLifetimeScope();
            builder.RegisterType<InformationMessageService>().As<IInformationMessageService>().InstancePerLifetimeScope();
            builder.RegisterType<LanguageVersionService>().As<ILanguageVersionService>().InstancePerLifetimeScope();
            builder.RegisterType<UserDeviceService>().As<IUserDeviceService>().InstancePerLifetimeScope();
            builder.RegisterType<DeletedAccountService>().As<IDeletedAccountService>().InstancePerLifetimeScope();
            builder.RegisterType<InternalValueService>().As<IInternalValueService>().InstancePerLifetimeScope();
            builder.RegisterType<DatabaseService>().As<IDatabaseService>().InstancePerLifetimeScope();

            builder.RegisterType<SpeechRecognitionService>().As<ISpeechRecognitionService>().InstancePerLifetimeScope();
            builder.RegisterType<WavFileService>().As<IWavFileService>().InstancePerLifetimeScope();
            builder.RegisterType<BillingPurchaseService>().As<IBillingPurchaseService>().InstancePerLifetimeScope();
            builder.RegisterType<AuthenticationService>().As<IAuthenticationService>().InstancePerLifetimeScope();
            builder.RegisterType<PushNotificationsService>().As<IPushNotificationsService>().InstancePerLifetimeScope();
            builder.RegisterType<CleanUpService>().As<ICleanUpService>().InstancePerLifetimeScope();
            builder.RegisterType<StorageService>().As<IStorageService>().InstancePerLifetimeScope();

            builder.RegisterType<SpeechRecognitionManager>().As<ISpeechRecognitionManager>().InstancePerLifetimeScope();
            builder.RegisterType<WavFileManager>().As<IWavFileManager>().InstancePerLifetimeScope();

            builder.RegisterType<MailService>().As<IMailService>().InstancePerLifetimeScope();
            builder.RegisterType<MessageCenterService>().As<IMessageCenterService>().InstancePerLifetimeScope();

            builder.RegisterType<CacheService>().As<ICacheService>().SingleInstance();
        }
    }
}
