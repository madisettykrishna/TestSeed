using SeedApp.Common.Enums;
using SeedApp.Common.Interfaces;
using SeedApp.Common.Logging;
using SeedApp.Common.Messages;
using Xamarin.Forms;

namespace SeedApp
{
	public partial class App : Application
	{
		private readonly INavigationService _navigationService;
		private readonly IConnectivityHelper _connectivityHelper;
		private readonly IMessagingService _messagingService;
		private readonly ILogger _logger;

		public App()
		{
			InitializeComponent();
		}

		public App(INavigationService navigationServices,
				   IConnectivityHelper connectivityHelper,
				   IMessagingService messagingService,
				   ILogger logger) : base()
		{
			_navigationService = navigationServices;
			_connectivityHelper = connectivityHelper;
			_messagingService = messagingService;
			_logger = logger;
		}

		protected override void OnStart()
		{
			// Handle when your app starts
			_logger.Info("App start", null, new[] { LoggerConstants.AppLifecycle });
			_connectivityHelper.InitiateCheckingAsync();
			_navigationService.NavigateToFirstScreenAsync();
		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
			_logger.Info("App sleep", null, new[] { LoggerConstants.AppLifecycle });

			_messagingService.Send(new AppLifecycleChangedMessage(AppLifecycleState.Sleep), "AppLifecyleChangeMessage");

			_connectivityHelper.PauseChecking();
		}

		protected override void OnResume()
		{
			// Handle when your app resumes
			_logger.Info("App resume", null, new[] { LoggerConstants.AppLifecycle });

			_messagingService.Send(new AppLifecycleChangedMessage(AppLifecycleState.Resume), "AppLifecyleChangeMessage");

			_connectivityHelper.ContinueChecking();
		}
	}
}
