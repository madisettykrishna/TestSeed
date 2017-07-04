using System;
using System.Threading.Tasks;
using Plugin.Connectivity;
using SeedApp.Common.Enums;
using SeedApp.Common.Interfaces;
using SeedApp.Common.Messages;

namespace SeedApp.Common.Providers
{
	public class ConnectivityHelper : IConnectivityHelper
	{
		private readonly IMessagingService _messagingService;
		private readonly INetworkStatusService _networkStatusService;
		private readonly IThreadExecutionProvider _threadExecutionProvider;
		private bool _isKeepChecking;
		private bool _isConnected;

		public ConnectivityHelper(IMessagingService messagingService,
								  IThreadExecutionProvider threadExecutionProvider,
								 INetworkStatusService networkStatusService)
		{
			_messagingService = messagingService;
			_threadExecutionProvider = threadExecutionProvider;
			_networkStatusService = networkStatusService;
			_isConnected = CrossConnectivity.Current.IsConnected;
			CrossConnectivity.Current.ConnectivityChanged += OnConnectivityChanged;
			_networkStatusService.NetworkStatusChanged += NetworkStatusService_NetworkStatusChanged;
		}

		public bool IsConnected
		{
			get
			{
				return _isConnected;
			}

			private set
			{
				if (_isConnected == value)
					return;

				_isConnected = value;

				_threadExecutionProvider.RunOnMainThread(() =>
				{
					_messagingService.Send(new ConnectivityChangedMessage(_isConnected));
				});
			}
		}

		public NetworkStatus NetworkStatus
		{
			get
			{
				var r = _networkStatusService.NetworkStatus();

				return r;
			}
		}

		public async Task InitiateCheckingAsync()
		{
			_isKeepChecking = true;

			while (true)
			{
				await Task.Delay(5000);
				if (_isKeepChecking)
					await SetConnectionAsync();
			}
		}

		public void ContinueChecking()
		{
			_isKeepChecking = true;
		}

		public void PauseChecking()
		{
			_isKeepChecking = false;
		}

		public async Task SetConnectionAsync()
		{
			await Task.Run(async () =>
			{
				IsConnected = CrossConnectivity.Current.IsConnected && await CrossConnectivity.Current.IsReachable("www.google.com");
			});
		}

		private async void OnConnectivityChanged(object sender, Plugin.Connectivity.Abstractions.ConnectivityChangedEventArgs e)
		{
			await SetConnectionAsync();
		}

		private void NetworkStatusService_NetworkStatusChanged(object sender, EventArgs e)
		{
			_messagingService.Send(new NetworkStatusChangedMessage());
		}
	}
}
