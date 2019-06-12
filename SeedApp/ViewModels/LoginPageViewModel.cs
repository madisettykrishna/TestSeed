using System;
using System.Threading.Tasks;
using System.Windows.Input;
using SeedApp.Common.Exception;
using SeedApp.Common.Interfaces;
using Xamarin.Forms;

namespace SeedApp.ViewModels
{
    public class LoginPageViewModel : BaseViewModel
    {
        private readonly IDialogService _dialogService;
        private readonly INavigationService _navigationService;
        private readonly IMemberPlusAuthProvider _authProvider;
        private readonly ISecurityManager _securityManager;
        private string _userName, _password;

        public LoginPageViewModel(INavigationService navigationService,
                                  IDialogService dialogService,
                                  IMemberPlusAuthProvider authProvider,
                                  ISecurityManager securityManager)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;
            _authProvider = authProvider;
            _securityManager = securityManager;

            LoginCommand = new Command(async () => await Login());
        }

        public string UserName
        {
            get { return _userName; }
            set { SetProperty(ref _userName, value); }
        }

        public string Password
        {
            get { return _password; }
            set { SetProperty(ref _password, value); }
        }

        public ICommand LoginCommand { get; private set; }

        private async Task Login()
        {
            if (!await ValidateFields())
                return;

            _dialogService.ShowProgress("Loading...");

            try
            {
                var accessToken = await _authProvider.LoginAsyc(UserName, Password);

                if (accessToken == null)
                {
                    _dialogService.HideProgress();

                    Data.Helpers.Settings.UserName = UserName;
                    Data.Helpers.Settings.Passowrd = Password;
                    Password = string.Empty;
                    await _dialogService.DisplayAlert("SeedApp", "Invalid Username or Password", "OK");
                    return;
                }

                _securityManager.SaveAccessToken(accessToken);
                await _navigationService.LoginStatusChanged();
            }
            catch (MemberPlusApiException ex)
            {
                _dialogService.HideProgress();
                await _dialogService.DisplayAlert("SeedApp", ex.Message, "Ok");
            }
            catch (Exception)
            {
                _dialogService.HideProgress();
                await _dialogService.DisplayAlert("SeedApp", "Unable to connect. Please check your internet connection and try again", "OK");
            }

            _dialogService.HideProgress();
        }

        private async Task<bool> ValidateFields()
        {
            if (string.IsNullOrWhiteSpace(Password) && string.IsNullOrWhiteSpace(Password))
            {
                _dialogService.HideProgress();
                await _dialogService.DisplayAlert("SeedApp", "Username and Password are required", "OK");
                return false;
            }

            if (string.IsNullOrWhiteSpace(UserName))
            {
                _dialogService.HideProgress();
                await _dialogService.DisplayAlert("SeedApp", "Please enter your Username.", "OK");
                return false;
            }

            if (string.IsNullOrWhiteSpace(Password))
            {
                _dialogService.HideProgress();
                await _dialogService.DisplayAlert("SeedApp", "Please enter your Password.", "OK");
                return false;
            }

            return true;
        }
    }
}
