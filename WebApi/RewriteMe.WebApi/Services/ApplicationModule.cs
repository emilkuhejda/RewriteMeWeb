using System.Reflection;
using Autofac;
using Autofac.Core;
using Autofac.Extensions.DependencyInjection;
using RewriteMe.Business.Managers;
using RewriteMe.Business.Services;
using RewriteMe.DataAccess;
using RewriteMe.DataAccess.Repositories;
using RewriteMe.Domain.Interfaces.Repositories;
using RewriteMe.Domain.Interfaces.Services;
using RewriteMe.Domain.Managers;
using Module = Autofac.Module;

namespace RewriteMe.WebApi.Services
{
    public class ApplicationModule : Module
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
            builder.RegisterType<TranscribeItemRepository>().As<ITranscribeItemRepository>().InstancePerLifetimeScope();
            builder.RegisterType<TranscribeItemSourceRepository>().As<ITranscribeItemSourceRepository>().InstancePerLifetimeScope();
            builder.RegisterType<RecognizedAudioSampleRepository>().As<IRecognizedAudioSampleRepository>().InstancePerLifetimeScope();
            builder.RegisterType<SpeechResultRepository>().As<ISpeechResultRepository>().InstancePerLifetimeScope();
            builder.RegisterType<UserSubscriptionRepository>().As<IUserSubscriptionRepository>().InstancePerLifetimeScope();
            builder.RegisterType<BillingPurchaseRepository>().As<IBillingPurchaseRepository>().InstancePerLifetimeScope();
            builder.RegisterType<ApplicationLogRepository>().As<IApplicationLogRepository>().InstancePerLifetimeScope();
            builder.RegisterType<ContactFormRepository>().As<IContactFormRepository>().InstancePerLifetimeScope();
            builder.RegisterType<InformationMessageRepository>().As<IInformationMessageRepository>().InstancePerLifetimeScope();
            builder.RegisterType<LanguageVersionRepository>().As<ILanguageVersionRepository>().InstancePerLifetimeScope();
            builder.RegisterType<UserDeviceRepository>().As<IUserDeviceRepository>().InstancePerLifetimeScope();
            builder.RegisterType<InternalValueRepository>().As<IInternalValueRepository>().InstancePerLifetimeScope();
            builder.RegisterType<DatabaseRepository>().As<IDatabaseRepository>().InstancePerLifetimeScope();

            builder.RegisterType<AdministratorService>().As<IAdministratorService>().InstancePerLifetimeScope();
            builder.RegisterType<UserService>().As<IUserService>().InstancePerLifetimeScope();
            builder.RegisterType<FileItemService>().As<IFileItemService>().InstancePerLifetimeScope();
            builder.RegisterType<FileItemSourceService>().As<IFileItemSourceService>().InstancePerLifetimeScope();
            builder.RegisterType<TranscribeItemService>().As<ITranscribeItemService>().InstancePerLifetimeScope();
            builder.RegisterType<TranscribeItemSourceService>().As<ITranscribeItemSourceService>().InstancePerLifetimeScope();
            builder.RegisterType<RecognizedAudioSampleService>().As<IRecognizedAudioSampleService>().InstancePerLifetimeScope();
            builder.RegisterType<SpeechResultService>().As<ISpeechResultService>().InstancePerLifetimeScope();
            builder.RegisterType<UserSubscriptionService>().As<IUserSubscriptionService>().InstancePerLifetimeScope();
            builder.RegisterType<ApplicationLogService>().As<IApplicationLogService>().InstancePerLifetimeScope();
            builder.RegisterType<ContactFormService>().As<IContactFormService>().InstancePerLifetimeScope();
            builder.RegisterType<InformationMessageService>().As<IInformationMessageService>().InstancePerLifetimeScope();
            builder.RegisterType<LanguageVersionService>().As<ILanguageVersionService>().InstancePerLifetimeScope();
            builder.RegisterType<UserDeviceService>().As<IUserDeviceService>().InstancePerLifetimeScope();
            builder.RegisterType<InternalValueService>().As<IInternalValueService>().InstancePerLifetimeScope();
            builder.RegisterType<DatabaseService>().As<IDatabaseService>().InstancePerLifetimeScope();

            builder.RegisterType<SpeechRecognitionService>().As<ISpeechRecognitionService>().InstancePerLifetimeScope();
            builder.RegisterType<WavFileService>().As<IWavFileService>().InstancePerLifetimeScope();
            builder.RegisterType<BillingPurchaseService>().As<IBillingPurchaseService>().InstancePerLifetimeScope();
            builder.RegisterType<AuthenticationService>().As<IAuthenticationService>().InstancePerLifetimeScope();
            builder.RegisterType<FileAccessService>().As<IFileAccessService>().InstancePerLifetimeScope();
            builder.RegisterType<PushNotificationsService>().As<IPushNotificationsService>().InstancePerLifetimeScope();
            builder.RegisterType<CleanUpService>().As<ICleanUpService>().InstancePerLifetimeScope();

            builder.RegisterType<SpeechRecognitionManager>().As<ISpeechRecognitionManager>().InstancePerLifetimeScope();
            builder.RegisterType<WavFileManager>().As<IWavFileManager>().InstancePerLifetimeScope();
        }
    }
}
