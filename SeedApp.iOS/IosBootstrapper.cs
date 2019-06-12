using Autofac;
using SeedApp.Common.Data;
using SeedApp.Common.Interfaces;
using SeedApp.Common.Logging;
using SeedApp.IOS.Providers;
using SeedApp.IOS.Services;
using SeedApp.SharedCode.Raygun;
using XLabs.Platform.Services;
using XLabs.Platform.Services.Media;

namespace SeedApp.IOS
{
    public class IosBootstrapper
    {
        public static void Initialize(ContainerBuilder builder)
        {
            RegisterProviders(builder);
        }

        private static void RegisterProviders(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<SecureStorageProvider>().As<ISecureStorageProvider>();
            containerBuilder.RegisterType<SecureStorage>().As<ISecureStorage>();
            containerBuilder.RegisterType<StackFrameProvider>().As<IStackFrameProvider>();
            containerBuilder.RegisterType<MemberPlusDataConfig>().As<IMemberPlusDataConfig>();
            containerBuilder.RegisterType<PlatformServiceIos>().As<IPlatformService>();
            containerBuilder.RegisterType<MediaPicker>().As<IMediaPicker>();
            containerBuilder.RegisterType<RaygunAppAnalyticsProvider>().As<IAppAnalyticsProvider>();
            containerBuilder.RegisterType<IosDialogService>().As<IDialogService>();
        }
    }
}