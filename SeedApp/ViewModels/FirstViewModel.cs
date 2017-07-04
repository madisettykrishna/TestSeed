using System;
using System.Threading.Tasks;
using System.Windows.Input;
using SeedApp.Common.Enums;
using SeedApp.Common.Interfaces;
using SeedApp.Common.Logging;
using SeedApp.Common.Messages;
using SeedApp.Data.Repositories.Interfaces;
using Xamarin.Forms;

namespace SeedApp.Forms.ViewModels
{
	public class FirstViewModel : BaseViewModel
	{
		private readonly ITestRepository _testRepository;
		private readonly IPlatformService _platformService;
		private readonly IMessagingService _messagingService;
		private readonly IConnectivityHelper _connectivityHelper;
		private readonly IDialogService _dialogService;

		private string _version;
		private bool _isConnected;
		private bool _okCancelAlertResponse;

		public FirstViewModel(
			INavigationService navigationService,
			ILogger logger,
			ITestRepository testRepository,
			IPlatformService platformService,
			IMessagingService messagingService,
			IConnectivityHelper connectivityHelper,
			IDialogService dialogService) : base(navigationService, logger)
		{
			_testRepository = testRepository;
			_platformService = platformService;
			_messagingService = messagingService;
			_connectivityHelper = connectivityHelper;
			_dialogService = dialogService;
			OkCancelAlertResponse = false;
			OSVersion = $"{_platformService.OsVersion.Major}.{_platformService.OsVersion.Minor}.{_platformService.OsVersion.Build}";
			IsConnected = connectivityHelper.IsConnected;
			logger.Verbose("First ViewModel", null, new[] { LoggerConstants.UiAction });
			logger.Verbose("OS Version", new { OSVersion }, new[] { LoggerConstants.UiAction });
			ShowDialog = new Command(DisplayProgressDialog);
			DisplayCancelAlert = new Command(DisplayAlertWithCancleButton);
			DisplayOkCancelAlert = new Command(DisplayAlertWithOkCancleButton);
			ShowModalPopupDialog = new Command(ShowModalPopup);
		}

		public async void OnAppear()
		{
			_messagingService.Subscribe<ConnectivityChangedMessage>(this, OnConnectivityChange);
		}

		public async void OnDisappear()
		{
			_messagingService.Unsubscribe<ConnectivityChangedMessage>(this);
		}

		public ICommand ShowDialog { get; set; }

		public ICommand DisplayCancelAlert { get; set; }

		public ICommand DisplayOkCancelAlert { get; set; }

		public ICommand ShowModalPopupDialog { get; set; }

		public string OSVersion
		{
			get { return _version; }

			private set { SetProperty(ref _version, value); }
		}

		public bool IsConnected
		{
			get { return _isConnected; }

			set { SetProperty(ref _isConnected, value); }
		}

		public bool OkCancelAlertResponse
		{
			get { return _okCancelAlertResponse; }

			set { SetProperty(ref _okCancelAlertResponse, value); }
		}


		public string AppName => _platformService.AppName;

		public string AppVersion => _platformService.AppVersion;

		public string BundleId => _platformService.BundleId;

		public string DeviceName => _platformService.DeviceName;

		public string DeviceId => _platformService.DeviceUuid;

		public string IPAddress => _platformService.IpAddress;

		public bool IsSimulator => _platformService.IsSimulator;

		public double SCreenScale => _platformService.ScreenScale;

		public double ScreenWidth => _platformService.ScreenSize.Width;

		public double ScreenHeight => _platformService.ScreenSize.Height;

		public double BatteryLevel => _platformService.BatteryLevel;

		public string SSIDName => _platformService.SsidName;

		public string WifiName => _platformService.WifiName;

		public NetworkStatus NetworkStatus => _connectivityHelper.NetworkStatus;

		private void OnConnectivityChange(object sender, ConnectivityChangedMessage message)
		{
			IsConnected = message.IsConnected;
		}

		private async void DisplayProgressDialog()
		{
			_dialogService.ShowProgress("Loading...");
			await Task.Delay(3000);
			_dialogService.HideProgress();
		}

		private async void DisplayAlertWithCancleButton()
		{
			await _dialogService.DisplayAlertAsync("First Page", "This is alert dialog with cancel button", "Cancel");
		}

		private async void DisplayAlertWithOkCancleButton()
		{
			OkCancelAlertResponse = await _dialogService.DisplayAlertAsync("First Page", "This is alert dialog with Ok and Cancel button", "Ok", "Cancel");
		}

		private async void ShowModalPopup()
		{
			StackLayout sampleStack = new StackLayout()
			{
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.Center,
				BackgroundColor = Color.Aqua,
				Margin = new Thickness(20),
				WidthRequest = _platformService.ScreenSize.Width,
				HeightRequest = 150,
			};

			Label lblTitle = new Label()
			{
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.Center,
				Text = "Title",
				TextColor = Color.Blue,
				WidthRequest = _platformService.ScreenSize.Width,
				HeightRequest = 40,
				HorizontalTextAlignment=TextAlignment.Center,
				VerticalTextAlignment = TextAlignment.Center
			};

			Label lblMessage = new Label()
			{
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.Center,
				Text = "Some Message or Content",
				TextColor = Color.Red,
				WidthRequest = _platformService.ScreenSize.Width,
				HeightRequest = 40,
				HorizontalTextAlignment=TextAlignment.Center,
				VerticalTextAlignment = TextAlignment.Center
			};

			Button btnClose = new Button()
			{
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.Center,
				Text = "Close",
				TextColor = Color.Green,
				WidthRequest = _platformService.ScreenSize.Width,
				HeightRequest = 40,
			};

			sampleStack.Children.Add(lblTitle);
			sampleStack.Children.Add(lblMessage);
			sampleStack.Children.Add(btnClose);

			_dialogService.ShowModalPopup(sampleStack);
			await Task.Delay(5000);
			_dialogService.HideModalPopup();
		}
	}
}
