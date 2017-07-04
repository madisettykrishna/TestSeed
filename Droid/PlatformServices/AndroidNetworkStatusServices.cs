using System;
using Android.App;
using Android.Content;
using Android.Net;
using Plugin.Connectivity;
using SeedApp.Common.Enums;
using SeedApp.Common.Interfaces;

namespace SeedApp.Forms.Droid.PlatformServices
{
	public class AndroidNetworkStatusServices : INetworkStatusService
	{		
		private NetworkStatus _currentNetworkStatus = Common.Enums.NetworkStatus.NotReachable;

		public AndroidNetworkStatusServices()
		{
			CrossConnectivity.Current.ConnectivityTypeChanged += Current_ConnectivityTypeChanged;
		}

		public event EventHandler NetworkStatusChanged;

		public NetworkStatus NetworkStatus()
		{
			return _currentNetworkStatus;
		}

		void Current_ConnectivityTypeChanged(object sender, Plugin.Connectivity.Abstractions.ConnectivityTypeChangedEventArgs e)
		{
			NetworkStatus status = Common.Enums.NetworkStatus.NotReachable;

			if (CrossConnectivity.Current.IsConnected)
			{
				if (CrossConnectivity.Current.ConnectionTypes.GetEnumerator().Current == Plugin.Connectivity.Abstractions.ConnectionType.WiFi)
					status = Common.Enums.NetworkStatus.ReachableViaWiFiNetwork;
				else if (CrossConnectivity.Current.ConnectionTypes.GetEnumerator().Current == Plugin.Connectivity.Abstractions.ConnectionType.Cellular)
					status = Common.Enums.NetworkStatus.ReachableViaCarrierDataNetwork;
			}
			else
				status = Common.Enums.NetworkStatus.NotReachable;

			if (_currentNetworkStatus != status)
			{
				_currentNetworkStatus = status;
				if (NetworkStatusChanged != null)
					NetworkStatusChanged(this, EventArgs.Empty);
			}
		}
	}
}
