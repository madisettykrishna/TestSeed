using Autofac;
using SeedApp.Common.Interfaces;
using SeedApp.Providers;
using SeedApp.Services;
using SeedApp.ViewModels;
using SeedApp.Views;

namespace SeedApp
{
    public class Bootstrapper
    {
        public static void Initialize(ContainerBuilder builder)
        {
            RegisterServices(builder);
            RegisterPages(builder);
            RegisterViewmodels(builder);
        }

        private static void RegisterServices(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<NavigationService>().As<INavigationService>().SingleInstance();
            containerBuilder.RegisterType<MessagingService>().As<IMessagingService>().SingleInstance();
            containerBuilder.RegisterType<DeviceProvider>().As<IDeviceProvider>().SingleInstance();
        }

        private static void RegisterPages(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<App>();
            containerBuilder.RegisterType<LoginPage>();
            containerBuilder.RegisterType<MenuPage>();
        }

        private static void RegisterViewmodels(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<BaseViewModel>();
            containerBuilder.RegisterType<LoginPageViewModel>();
            containerBuilder.RegisterType<MenuPageViewModel>();
        }
    }
}