using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Content.Res;
using Android.Net;
using Android.OS;
using Mindscape.Raygun4Net;
using SeedApp.Common;
using SeedApp.Common.Interfaces;
using SeedApp.Data;
using SeedApp.Forms;
using SeedApp.Forms.Droid;
using SeedApp.Forms.Droid.BroadcastReceiver;
using Xamarin.Forms;

namespace SeedApp.Droid
{
	[Activity(Label = "SeedApp.Droid", Icon = "@drawable/icon", Theme = "@style/MyTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
	{
		private NetworkStatusBroadcastReceiver _networkStatusBroadcastReceiver;
		private INetworkStatusService _networkStatusService;
		public static Size ScreenSize;
		public static int StatusBarHeight;
		public static int ActionBarHeight;
		public static ContentResolver AppContentResolver;

		protected override void OnCreate(Bundle bundle)
		{
			TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar;

			base.OnCreate(bundle);

			WireupDependency();

			ScreenSize = new Size(ConvertPixelsToDp(Resources.DisplayMetrics.WidthPixels), ConvertPixelsToDp(Resources.DisplayMetrics.HeightPixels));

			int StatusBarHeightResourceId = Resources.GetIdentifier("status_bar_height", "dimen", "android");
			StatusBarHeight = StatusBarHeightResourceId > 0 ? Resources.GetDimensionPixelSize(StatusBarHeightResourceId) : 0;

			TypedArray styledAttributes = this.Theme.ObtainStyledAttributes(new int[] { Android.Resource.Attribute.ActionBarSize });
			ActionBarHeight = (int)styledAttributes.GetDimension(0, 0);

			AppContentResolver = this.ContentResolver;

			//			var androidAppConfig = (IAppConfig)ContainerManager.Container.Resolve(typeof(IAppConfig), typeof(IAppConfig).GetType().ToString());

			//#if (APPSTORE || PLAYSTORE)
			//            RaygunClient.Attach(androidAppConfig.StoreAppRaygunKey);
			//#elif (BETA)
			//            RaygunClient.Attach(androidAppConfig.BetaAppRaygunKey);
			//#else
			//			RaygunClient.Attach(androidAppConfig.TestAppRaygunKey);
			//#endif

			global::Xamarin.Forms.Forms.Init(this, bundle);

			var application = (Xamarin.Forms.Application)ContainerManager.Container.Resolve(typeof(App), typeof(App).GetType().ToString());
			LoadApplication(application);
		}

		protected override void OnStart()
		{
			base.OnStart();

			/*if (_networkStatusService == null)
			{
				_networkStatusService = (INetworkStatusService)ContainerManager.Container.Resolve(typeof(INetworkStatusService), typeof(INetworkStatusService).GetType().ToString());
			}

			if (_networkStatusBroadcastReceiver == null)
			{
				_networkStatusBroadcastReceiver = new NetworkStatusBroadcastReceiver();
				_networkStatusBroadcastReceiver.ConnectionStatusChanged += NetworkStatusBroadcastReceiver_ConnectionStatusChanged;
			}

			// Register the broadcast receiver
			Android.App.Application.Context.RegisterReceiver(_networkStatusBroadcastReceiver, new IntentFilter(ConnectivityManager.ConnectivityAction));*/
		}

		protected override void OnStop()
		{
			base.OnStop();

			if (_networkStatusBroadcastReceiver != null)
			{
				// Unregister the receiver so we no longer get updates.
				Android.App.Application.Context.UnregisterReceiver(_networkStatusBroadcastReceiver);
				_networkStatusBroadcastReceiver.ConnectionStatusChanged -= NetworkStatusBroadcastReceiver_ConnectionStatusChanged;
				_networkStatusBroadcastReceiver = null;
			}
		}

		private void NetworkStatusBroadcastReceiver_ConnectionStatusChanged(object sender, EventArgs e)
		{
			_networkStatusService.NetworkStatus();
		}

		private void WireupDependency()
		{
			// Wire up dependencies of core classes
			ContainerManager.Initialize();
			CommonBootstrapper.Initialize(ContainerManager.Container);
			DataBootstrapper.Initialize(ContainerManager.Container);
			FormsBootstrapper.Initialize(ContainerManager.Container);
			AndroidBootstrapper.Initialize(ContainerManager.Container);
		}

		private int ConvertPixelsToDp(float pixelValue)
		{
			var dp = (int)((pixelValue) / Resources.DisplayMetrics.Density);
			return dp;
		}
	}
}
