using Autofac;
using SeedApp.Common.Data;
using SeedApp.Common.Interfaces;
using SeedApp.Common.Logging;
using SeedApp.Droid.Providers;
using SeedApp.Droid.Services;
using SeedApp.SharedCode.Raygun;

namespace SeedApp.Droid
{
    public class DroidBootstrapper
    {
        public static void Initialize(ContainerBuilder builder)
        {
            RegisterProviders(builder);
        }

        private static void RegisterProviders(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<MemberPlusDataConfig>().As<IMemberPlusDataConfig>().OwnedByLifetimeScope();
            containerBuilder.RegisterType<PlatformServiceDroid>().As<IPlatformService>().OwnedByLifetimeScope();
            containerBuilder.RegisterType<ApplicationInfoProvider>().As<IApplicationInfoProvider>().OwnedByLifetimeScope();
            containerBuilder.RegisterType<SecureStorageProvider>().As<ISecureStorageProvider>().OwnedByLifetimeScope();
            containerBuilder.RegisterType<StackFrameProvider>().As<IStackFrameProvider>().OwnedByLifetimeScope();
            containerBuilder.RegisterType<RaygunAppAnalyticsProvider>().As<IAppAnalyticsProvider>().OwnedByLifetimeScope();
            containerBuilder.RegisterType<AndroidDialogService>().As<IDialogService>().OwnedByLifetimeScope();
        }
    }
}