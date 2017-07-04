using System;
using System.Net;
using CoreFoundation;
using SeedApp.Common.Enums;
using SeedApp.Common.Interfaces;
using SystemConfiguration;

namespace SeedApp.Forms.iOS.PlatformServices
{
	public class IOSNetworkStatusServices : INetworkStatusService
	{
		private NetworkReachability defaultRouteReachability;
		private NetworkStatus _currentNetworkStatus;

		public IOSNetworkStatusServices()
		{
			NetworkStatus();
		}

		public event EventHandler NetworkStatusChanged;

		public NetworkStatus NetworkStatus()
		{
			NetworkStatus status = Common.Enums.NetworkStatus.NotReachable;

			NetworkReachabilityFlags flags;
			bool defaultNetworkAvailable = IsNetworkAvailable(out flags);

			// If the connection is reachable and no connection is required, then assume it's WiFi
			if (defaultNetworkAvailable)
			{
				status = Common.Enums.NetworkStatus.ReachableViaWiFiNetwork;
			}

			// If the connection is on-demand or on-traffic and no user intervention
			// is required, then assume WiFi.
			if (((flags & NetworkReachabilityFlags.ConnectionOnDemand) != 0
				|| (flags & NetworkReachabilityFlags.ConnectionOnTraffic) != 0)
				&& (flags & NetworkReachabilityFlags.InterventionRequired) == 0)
			{
				status = Common.Enums.NetworkStatus.ReachableViaWiFiNetwork;
			}

			// If it's a WWAN connection..
			if ((flags & NetworkReachabilityFlags.IsWWAN) != 0)
				status = Common.Enums.NetworkStatus.ReachableViaCarrierDataNetwork;

			_currentNetworkStatus = status;
			return status;
		}

		public void Dispose()
		{
			if (defaultRouteReachability != null)
			{
				defaultRouteReachability.Dispose();
				defaultRouteReachability = null;
			}
		}

		private bool IsNetworkAvailable(out NetworkReachabilityFlags flags)
		{
			if (defaultRouteReachability == null)
			{
				var ip = new IPAddress(0);
				defaultRouteReachability = new NetworkReachability(ip);
				defaultRouteReachability.SetNotification(OnChange);
				defaultRouteReachability.Schedule(CFRunLoop.Main, CFRunLoop.ModeDefault);
			}

			if (!defaultRouteReachability.TryGetFlags(out flags))
				return false;
			return IsReachableWithoutRequiringConnection(flags);
		}

		private bool IsReachableWithoutRequiringConnection(NetworkReachabilityFlags flags)
		{
			// Is it reachable with the current network configuration?
			bool isReachable = (flags & NetworkReachabilityFlags.Reachable) != 0;

			// Do we need a connection to reach it?
			bool noConnectionRequired = (flags & NetworkReachabilityFlags.ConnectionRequired) == 0;

			// Since the network stack will automatically try to get the WAN up,
			// probe that
			if ((flags & NetworkReachabilityFlags.IsWWAN) != 0)
				noConnectionRequired = true;

			return isReachable && noConnectionRequired;
		}

		private void OnChange(NetworkReachabilityFlags flags)
		{
			var h = NetworkStatusChanged;
			if (h != null)
				h(null, EventArgs.Empty);
		}
	}
}
