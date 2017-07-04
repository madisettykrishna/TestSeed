using System;
using Xamarin.Forms;

namespace SeedApp.Common.Interfaces
{
	public interface IPlatformService
	{
		string AppVersion { get; }

		string BundleId { get; }

		string AppName { get; }

		double BatteryLevel { get; }

		string DeviceName { get; }

		string DeviceUuid { get; }

		Version OsVersion { get; }

		string IpAddress { get; }

		double ScreenBrightness { get; set; }

		double ScreenScale { get; }

		Size ScreenSize { get; }

		float NavigationBarHeight { get; }

		float StatusBarHeight { get; }

		int CurrentThreadId { get; }

		bool IsSimulator { get; }

		string SsidName { get; }

		string WifiName { get; }
	}
}
