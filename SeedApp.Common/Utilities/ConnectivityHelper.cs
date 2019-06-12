using System.Threading.Tasks;
using SeedApp.Common.Interfaces;
using SeedApp.Common.Messages;
using Plugin.Connectivity;
using Xamarin.Forms;

namespace SeedApp.Common.Utilities
{
    public class ConnectivityHelper : IConnectivityHelper
    {
        private readonly IMessagingService _messagingService;
        private readonly IMemberPlusAppConfig _appConfig;
        private bool _isKeepChecking;
        private bool _isConnected;

        public ConnectivityHelper(IMessagingService messagingService, IMemberPlusAppConfig appConfig)
        {
            _messagingService = messagingService;
            _appConfig = appConfig;
            _isConnected = CrossConnectivity.Current.IsConnected;
            CrossConnectivity.Current.ConnectivityChanged += OnConnectivityChanged;
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
                Device.BeginInvokeOnMainThread(() => 
                {
                    _messagingService.Send(new ConnectivityChangedMessage(_isConnected)); 
                });
            }
        }

        public async Task InitiateCheckingAsync()
        {
            _isKeepChecking = true;

            while (true)
            {
                await Task.Delay(1000);
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

        private async void OnConnectivityChanged(object sender, Plugin.Connectivity.Abstractions.ConnectivityChangedEventArgs e)
        {
            await SetConnectionAsync();
        }

        private async Task SetConnectionAsync()
        {
            await Task.Run(async () =>
            {
                IsConnected = CrossConnectivity.Current.IsConnected && await CrossConnectivity.Current.IsRemoteReachable(_appConfig.ServerDataUrl);
            });
        }
    }
}