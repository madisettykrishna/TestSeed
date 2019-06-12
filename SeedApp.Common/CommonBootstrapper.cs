using Autofac;
using SeedApp.Common.Interfaces;
using SeedApp.Common.Logging;
using SeedApp.Common.Utilities;

namespace SeedApp.Common
{
    internal class CommonBootstrapper
    {
        public static void Initialize(ContainerBuilder builder)
        {
            RegisterServices(builder);
            RegisterTypes(builder);
        }

        private static void RegisterServices(ContainerBuilder containerBuilder)
        {
        }

        private static void RegisterTypes(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<Logger>().As<ILogger>().SingleInstance();
            containerBuilder.RegisterType<ConnectivityHelper>().As<IConnectivityHelper>().SingleInstance();
        }
    }
}