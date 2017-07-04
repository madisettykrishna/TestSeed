using System;
using Microsoft.Practices.Unity;
using SeedApp.Common.Interfaces;
using SeedApp.Forms.iOS.PlatformServices;
using SeedApp.Forms.iOS.Utils;

namespace SeedApp.Forms.iOS
{
	public class IosBootstrapper
	{
		public static void Initialize(IUnityContainer container)
		{
			// Interface and Class Registration goes here.
			container.RegisterType<IAppConfig, IosAppConfig>(new TransientLifetimeManager());
			container.RegisterType<INetworkStatusService, IOSNetworkStatusServices>(new HierarchicalLifetimeManager());
			container.RegisterType<IThreadExecutionProvider, ThreadExecutionProvider>(new TransientLifetimeManager());
			container.RegisterType<IStackFrameHelper, StackFrameHelper>(new ContainerControlledLifetimeManager());
			container.RegisterType<IPlatformService, PlatformServiceIos>(new ContainerControlledLifetimeManager());
			container.RegisterType<IGoogleAnalyticsServices, IosGoogleAnalyticsServices>(new ContainerControlledLifetimeManager());
			container.RegisterType<IDialogService, IosDialogService>(new TransientLifetimeManager());
		}
	}
}