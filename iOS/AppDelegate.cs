using System;
using System.Threading.Tasks;
using Foundation;
using SeedApp.Common;
using SeedApp.Common.Logging;
using SeedApp.Data;
using SeedApp.Forms;
using SeedApp.Forms.iOS;
using UIKit;

namespace SeedApp.iOS
{
	[Register("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
			UIDevice.CurrentDevice.BatteryMonitoringEnabled = true;

			AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

			TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;

			//Gai.SharedInstance.DispatchInterval = 60;
			//Gai.SharedInstance.TrackUncaughtExceptions = false;
			//Gai.SharedInstance.GetTracker(IosAppConfig.GoogleAnalyticsId);

			global::Xamarin.Forms.Forms.Init();

			WireupDependency();

			var application = (Xamarin.Forms.Application)ContainerManager.Container.Resolve(typeof(App), typeof(App).GetType().ToString());
			LoadApplication(application);

			return base.FinishedLaunching(app, options);
		}

		public override void DidEnterBackground(UIApplication application)
		{
			UIDevice.CurrentDevice.BatteryMonitoringEnabled = false;
			UIApplication.SharedApplication.NetworkActivityIndicatorVisible = false;
			application.ApplicationIconBadgeNumber = 0;
		}

		public override void WillEnterForeground(UIApplication application)
		{
			UIDevice.CurrentDevice.BatteryMonitoringEnabled = true;
		}

		private void WireupDependency()
		{
			// Wire up dependencies of core classes
			ContainerManager.Initialize();
			CommonBootstrapper.Initialize(ContainerManager.Container);
			DataBootstrapper.Initialize(ContainerManager.Container);
			FormsBootstrapper.Initialize(ContainerManager.Container);
			IosBootstrapper.Initialize(ContainerManager.Container);
		}

		private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			var logger = (SeedApp.Common.Logging.ILogger)ContainerManager.Container.Resolve(typeof(SeedApp.Common.Logging.ILogger), typeof(SeedApp.Common.Logging.ILogger).GetType().ToString());
			logger.Error("Unhandled exception occurred", e.ExceptionObject, new[] { LoggerConstants.UnhandledException });

			throw (System.Exception)e.ExceptionObject;
		}

		private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
		{
			var logger = (SeedApp.Common.Logging.ILogger)ContainerManager.Container.Resolve(typeof(SeedApp.Common.Logging.ILogger), typeof(SeedApp.Common.Logging.ILogger).GetType().ToString());
			logger.Error("Unhandled exception occurred", e.Exception, new[] { LoggerConstants.UnhandledException });

			throw e.Exception;
		}
	}
}
