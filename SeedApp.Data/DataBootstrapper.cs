using Autofac;
using SeedApp.Common.Data;
using SeedApp.Common.Interfaces;
using SeedApp.Common.Logging;
using SeedApp.Data.Logging;
using SeedApp.Data.Managers;
using SeedApp.Data.Providers;
using SeedApp.Data.Services;

namespace SeedApp.Data
{
    public class DataBootstrapper
    {
        public static void Initialize(ContainerBuilder builder)
        {
            RegisterDatabase(builder);
            RegisterProviders(builder);
            RegisterManagers(builder);
            RegisterServices(builder);
        }

        private static void RegisterDatabase(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<AppDatabase>().As<IAppDatabase>().SingleInstance();
            containerBuilder.RegisterType<ContactsDatabase>().As<IContactsDatabase>().SingleInstance();
            containerBuilder.RegisterType<LogDatabase>().As<ILogDatabase>().SingleInstance();
        }

        private static void RegisterProviders(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<LocalDbLoggingProvider>().As<ILoggingProvider>().SingleInstance();
            containerBuilder.RegisterType<MemberPlusAuthProvider>().As<IMemberPlusAuthProvider>();
            containerBuilder.RegisterType<MemberPlusAppConfig>().As<IMemberPlusAppConfig>();
        }

        private static void RegisterManagers(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<SecurityManager>().As<ISecurityManager>();
            containerBuilder.RegisterType<PeriodicSyncManager>().As<IPeriodicSyncManager>().SingleInstance();
        }

        private static void RegisterServices(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<MemberPlusApiService>().As<IMemberPlusApiService>();
        }
    }
}