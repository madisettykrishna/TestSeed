using SeedApp.ViewModels;
using Xamarin.Forms;

namespace SeedApp.Views
{
    public partial class LoginPage : ContentPage
    {
        private readonly LoginPageViewModel _loginPageViewModel;

        public LoginPage(LoginPageViewModel loginPageViewModel)
        {
            InitializeComponent();
            _loginPageViewModel = loginPageViewModel;
            BindingContext = _loginPageViewModel;
            NavigationPage.SetHasNavigationBar(this, false);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _loginPageViewModel.OnViewAppear();
        }
    }
}
