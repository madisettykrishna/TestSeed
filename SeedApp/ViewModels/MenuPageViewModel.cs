using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using SeedApp.Common.Exception;
using SeedApp.Common.Interfaces;
using SeedApp.Models;
using Xamarin.Forms;

namespace SeedApp.ViewModels
{
    public class MenuPageViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly IDialogService _dialogService;
        private readonly ISecurityManager _securityManager;
        private MenuInfo _selectedMenuItem;

        public MenuPageViewModel(INavigationService navigationService,
                                 IDialogService dialogService,
                                 ISecurityManager securityManager)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;
            _securityManager = securityManager;
            _dialogService.HideProgress();
            BackgroundColor = Color.Black;
            MenuItems = new ObservableCollection<MenuInfo>();
            CurrentMenuPageViewModel = this;
            SelectMenuItemCommand = new Command(async () => await SelectMenuItemAsync());
            CreateMenuItemsAsync();
        }

        public static MenuPageViewModel CurrentMenuPageViewModel { get; set; } = null;

        public string ContactsText { get; set; }

        public MenuInfo SelectedMenuItem
        {
            get { return _selectedMenuItem; }

            set { SetProperty(ref _selectedMenuItem, value); }
        }

        public ICommand SelectMenuItemCommand { get; private set; }

        public ObservableCollection<MenuInfo> MenuItems { get; }

        public override async Task OnViewAppear()
        {
            await base.OnViewAppear();
        }

        public async Task CreateMenuItemsAsync()
        {
            try
            {
                MenuItems.Clear();
                MenuItems.Add(new MenuInfo { Title = "Dashboard", Id = 1, Icon = "Icon_Home.png" });
                MenuItems.Add(new MenuInfo { Title = "Logout", Id = 2, Icon = "Icon_Logout.png" });
            }
            catch (MemberPlusApiException ex)
            {
                await _dialogService.DisplayAlert("SeedApp", ex.UserMessage, "Ok");
            }
        }

        private async Task SelectMenuItemAsync()
        {
            if (SelectedMenuItem == null)
            {
                return;
            }

            switch (SelectedMenuItem.Id)
            {
                case 1:
                    await Dashboard();
                    break;
                case 2:
                    await Logout();
                    break;
                default:
                    break;
            }

            SelectedMenuItem = null;
        }

        private async Task Logout()
        {
            (App.Current.MainPage as MasterDetailPage).IsPresented = false;

            if (!await _dialogService.DisplayAlert("Logout", "Are you sure you want to Logout?", "No", "Yes"))
            {
                _dialogService.ShowProgress("Logging out...");
                _securityManager.Logout();
                _dialogService.HideProgress();
                await _navigationService.NavigateTo(PageKey.LoginPage, true);
            }
            else
            {
                SelectedMenuItem = null;
            }
        }

        private async Task Dashboard()
        {
            await _navigationService.NavigateTo(PageKey.Dashboard, true);
        }
    }
}
