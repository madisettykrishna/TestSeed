using System;
using System.Threading.Tasks;
using SeedApp.Common.Interfaces;
using SeedApp.Common.Logging;
using SeedApp.Common.Messages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace SeedApp
{
    public partial class App : Application
    {
        private readonly INavigationService _navigationService;
        private readonly IConnectivityHelper _connectivityHelper;
        private readonly IMessagingService _messagingService;
        private readonly ILogger _logger;
        private readonly IPeriodicSyncManager _periodicSyncManager;
        private readonly ISecurityManager _securityManager;
        private readonly IDialogService _dialogService;

        public App(INavigationService navigationService,
                   IConnectivityHelper connectivityHelper,
                   IMessagingService messagingService,
                   ILogger logger,
                   IPeriodicSyncManager periodicSyncManager,
                   ISecurityManager securityManager, 
                   IDialogService dialogService)
        {
            InitializeComponent();
            _navigationService = navigationService;
            _connectivityHelper = connectivityHelper;
            _messagingService = messagingService;
            _periodicSyncManager = periodicSyncManager;
            _logger = logger;
            _securityManager = securityManager;
            _dialogService = dialogService;

            ////The root page of your application
            Current = this;
            _navigationService.LoginStatusChanged();
        }

        public static new App Current { get; private set; }

        protected async override void OnStart()
        {
            try
            {
                _connectivityHelper.InitiateCheckingAsync();
                _periodicSyncManager.InitiateSyncing();
                await Task.Delay(500);
            }
            catch (Exception ex)
            {
                _logger.Exception(ex);
            }
        }

        protected override void OnSleep()
        {
            _messagingService.Send(new AppSleepMessage());
        }

        protected async override void OnResume()
        {
            _messagingService.Send(new AppResumeMessage());
        }
    }
}