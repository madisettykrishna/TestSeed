using System;
using Android.Bluetooth;
using Android.Telephony;
using Android.Content;
using SeedApp.Common.Interfaces;
using Xamarin.Forms;
using Android.Content.PM;
using Android.OS;
using System.Threading;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using Android.Net.Wifi;
using Android.Net;
using SeedApp.Droid;

namespace SeedApp.Forms.Droid.PlatformServices
{
	public class PlatformServiceAndroid : IPlatformService
	{
		private TelephonyManager _telephonyManager;
		private WifiManager _wifiManager;
		private Version _osVersion;
		private string _ssidName = string.Empty;

		public PlatformServiceAndroid()
		{
			_telephonyManager = (TelephonyManager)Android.App.Application.Context.GetSystemService(Context.TelephonyService);
			_osVersion = new Version(Build.VERSION.Release);
		}

		public string AppName
		{
			get
			{
				string packageName = Android.App.Application.Context.PackageName;
				return packageName.Substring(packageName.LastIndexOf('.'));
			}
		}

		public string AppVersion
		{
			get
			{
				PackageInfo packageInfo = Android.App.Application.Context.PackageManager.GetPackageInfo(Android.App.Application.Context.PackageName, 0);
				return $"{packageInfo.VersionName.ToString()}({packageInfo.VersionCode})";
			}
		}

		public double BatteryLevel
		{
			get
			{
				try
				{
					using (var filter = new IntentFilter(Intent.ActionBatteryChanged))
					{
						using (var battery = Android.App.Application.Context.RegisterReceiver(null, filter))
						{
							var level = battery.GetIntExtra(BatteryManager.ExtraLevel, -1);
							var scale = battery.GetIntExtra(BatteryManager.ExtraScale, -1);

							return Math.Floor(level * 100D / scale);
						}
					}
				}
				catch
				{
					System.Diagnostics.Debug.WriteLine("Ensure you have android.permission.BATTERY_STATS");
					throw;
				}
			}
		}

		public string BundleId => Android.App.Application.Context.PackageName;

		public int CurrentThreadId => Thread.CurrentThread.ManagedThreadId;

		public string DeviceName => BluetoothAdapter.DefaultAdapter.Name;

		public string DeviceUuid => _telephonyManager.DeviceId;

		public string IpAddress
		{
			get
			{
				foreach (var netInterface in NetworkInterface.GetAllNetworkInterfaces())
				{
					if (netInterface.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 ||
						netInterface.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
					{
						foreach (var addrInfo in netInterface.GetIPProperties().UnicastAddresses)
						{
							if (addrInfo.Address.AddressFamily == AddressFamily.InterNetwork)
							{
								var ipAddress = addrInfo.Address;

								return ipAddress.ToString();
							}
						}
					}
				}

				return string.Empty;
			}
		}

		public bool IsSimulator => Build.Product.Contains("sdk") || Build.Product.Contains("generic") || Build.Product.Contains("genymotion");

		public float NavigationBarHeight => MainActivity.ActionBarHeight;

		public Version OsVersion => _osVersion;

		public double ScreenBrightness
		{
			get
			{
				return Android.Provider.Settings.System.GetInt(MainActivity.AppContentResolver, Android.Provider.Settings.System.ScreenBrightness, 0);
			}

			set
			{
				Android.Provider.Settings.System.PutInt(MainActivity.AppContentResolver, Android.Provider.Settings.System.ScreenBrightness, (int)value);
			}
		}

		public double ScreenScale => 1;

		public Size ScreenSize => MainActivity.ScreenSize;

		public string SsidName
		{
			get
			{
				return GetSSIDName();
			}
		}

		public float StatusBarHeight => MainActivity.StatusBarHeight;

		public string WifiName
		{
			get
			{
				return GetSSIDName();
			}
		}

		private string GetSSIDName()
		{
			ConnectivityManager connectivityManager = (ConnectivityManager)Android.App.Application.Context.GetSystemService(Context.ConnectivityService);
			NetworkInfo networkInfo = connectivityManager.ActiveNetworkInfo;
			if (networkInfo != null && networkInfo.IsConnected)
			{
				WifiManager wifiManager = (WifiManager)(Android.App.Application.Context.GetSystemService(Context.WifiService));
				WifiInfo wifiInfo = wifiManager.ConnectionInfo;
				return wifiInfo != null && !string.IsNullOrEmpty(wifiInfo.SSID) ? wifiInfo.SSID : string.Empty;
			}

			return string.Empty;
		}
	}
}
