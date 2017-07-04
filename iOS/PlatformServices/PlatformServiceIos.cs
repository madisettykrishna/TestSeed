using System;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;
using Foundation;
using SeedApp.Common.Interfaces;
using SystemConfiguration;
using UIKit;
using Xamarin.Forms;

namespace SeedApp.Forms.iOS.PlatformServices
{
	public class PlatformServiceIos : IPlatformService
	{
		private readonly Lazy<string> _lazyAppVersion;
		private readonly Lazy<Version> _lazyOsVersion;
		private readonly Lazy<string> _lazyBundleId;
		private readonly Lazy<string> _lazyAppName;

		public PlatformServiceIos()
		{
			_lazyAppVersion = new Lazy<string>(() =>
			{
				if (NSBundle.MainBundle.InfoDictionary == null)
					return string.Empty;

				NSObject versionObject;
				NSObject buildObject;
				var build = NSBundle.MainBundle.InfoDictionary.TryGetValue((NSString)"CFBundleVersion",
					out buildObject)
					? buildObject.ToString()
					: string.Empty;

				var version = NSBundle.MainBundle.InfoDictionary.TryGetValue((NSString)"CFBundleShortVersionString",
					  out versionObject)
					  ? versionObject.ToString()
					  : string.Empty;

				return $"{version}.{build}";
			});

			_lazyOsVersion = new Lazy<Version>(() => new Version(UIDevice.CurrentDevice.SystemVersion));

			// This will obtain the BundleId of the app.
			_lazyBundleId = new Lazy<string>(() =>
			{
				NSObject bundleId;
				return NSBundle.MainBundle.InfoDictionary.TryGetValue((NSString)"CFBundleIdentifier", out bundleId) ? bundleId.ToString() : string.Empty;
			});

			_lazyAppName = new Lazy<string>(() =>
			{
				NSObject bundleName;
				return NSBundle.MainBundle.InfoDictionary.TryGetValue((NSString)"CFBundleName", out bundleName) ? bundleName.ToString() : string.Empty;
			});
		}

		public static float NavBarHeight { get; set; } = 0;

		public double ScreenScale => UIScreen.MainScreen.Scale;

		public string AppVersion => _lazyAppVersion.Value;

		public string AppName => _lazyAppName.Value;

		// This will return BundleId.
		public string BundleId => _lazyBundleId.Value;

		public double ScreenBrightness
		{
			get
			{
				return UIScreen.MainScreen.Brightness;
			}

			set
			{
				UIScreen.MainScreen.Brightness = (nfloat)value;
			}
		}

		public Size ScreenSize
			=> new Size(UIScreen.MainScreen.Bounds.Size.Width, UIScreen.MainScreen.Bounds.Size.Height);

		public string DeviceUuid => UIDevice.CurrentDevice.IdentifierForVendor.AsString();

		public string DeviceName => UIDevice.CurrentDevice.Name;

		public Version OsVersion => _lazyOsVersion.Value;

		public double BatteryLevel => Math.Abs(UIDevice.CurrentDevice.BatteryLevel * 100d);

		public int CurrentThreadId => Thread.CurrentThread.ManagedThreadId;

		public string SsidName
		{
			get
			{
				string[] supportedInterfaces = null;

				var result = CaptiveNetwork.TryGetSupportedInterfaces(out supportedInterfaces);

				if (result == StatusCode.OK && supportedInterfaces != null && supportedInterfaces.Any())
				{
					NSDictionary networkInfo;

					result = CaptiveNetwork.TryCopyCurrentNetworkInfo(supportedInterfaces[0], out networkInfo);

					if (result == StatusCode.OK)
					{
						if (networkInfo != null)
						{
							return networkInfo["SSID"] != null ? networkInfo["SSID"].ToString() : string.Empty;
						}
					}
				}

				return string.Empty;
			}
		}

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

		public bool IsSimulator => ObjCRuntime.Runtime.Arch == ObjCRuntime.Arch.SIMULATOR;

		public string WifiName
		{
			get
			{
				NSDictionary dict;
				CaptiveNetwork.TryCopyCurrentNetworkInfo("en0", out dict);
				if (dict == null || dict[CaptiveNetwork.NetworkInfoKeySSID] == null)
					return string.Empty;

				return dict[CaptiveNetwork.NetworkInfoKeySSID].ToString();
			}
		}

		public float NavigationBarHeight
		{
			get
			{
				return NavBarHeight;
			}
		}

		public float StatusBarHeight
		{
			get
			{
				return (float)UIApplication.SharedApplication.StatusBarFrame.Height;
			}
		}
	}
}
