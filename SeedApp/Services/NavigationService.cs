using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac;
using SeedApp.Common;
using SeedApp.Common.Interfaces;
using SeedApp.Views;
using Xamarin.Forms;

namespace SeedApp.Services
{
    public class NavigationService : INavigationService
    {
        private readonly ISecurityManager _securityManager;
        private readonly Dictionary<Type, PageKey> _pages;
        private Stack<PageKey> _navStack;
        private NavigationPage _navigation;

        public NavigationService(ISecurityManager securityManager)
        {
            _securityManager = securityManager;
            _pages = new Dictionary<Type, PageKey>();
            _navStack = new Stack<PageKey>();
        }

        public PageKey CurrentPageKey => _pages[_navigation.CurrentPage.GetType()];

        public bool HasBackPage => _navStack.Count > 1 || _navigation.Navigation.NavigationStack.Count > 1;

        public async Task LoginStatusChanged()
        {
            if (_securityManager.IsLoggedIn)
            {
                await NavigateTo(PageKey.Dashboard);
            }
            else
            {
                await NavigateTo(PageKey.LoginPage);
            }
        }

        public async Task NavigateTo(PageKey pageKey, bool animated = false)
        {
            var page = GetPage(pageKey);

            if (pageKey == PageKey.LoginPage)
            {
                _navigation = new NavigationPage(page)
                {
                    BackgroundColor = Color.FromHex("#3c8dbc"),
                    BarBackgroundColor = Color.FromHex("#3c8dbc"),
                    BarTextColor = Color.White,
                };

                App.Current.MainPage = _navigation;
                _navStack.Push(pageKey);
            }
            else
            {
                if (pageKey == PageKey.Dashboard)
                {
                    MasterDetailPage masterDetailPage = new MasterDetailPage();
                    MenuPage menuPage = ContainerManager.Container.Resolve<MenuPage>();

                    masterDetailPage.Master = menuPage;

                    _navigation = new NavigationPage(page)
                    {
                        BackgroundColor = Color.FromHex("#3c8dbc"),
                        BarBackgroundColor = Color.FromHex("#3c8dbc"),
                        BarTextColor = Color.White,
                    };

                    masterDetailPage.Detail = _navigation;
                    masterDetailPage.MasterBehavior = MasterBehavior.Popover;
                    App.Current.MainPage = masterDetailPage;
                    _navStack.Push(pageKey);
                }
                else
                {
                    await _navigation.PushAsync(page, animated);
                }
            }
        }

        public async Task GoBack(int numberOfPagesToSkip = 0)
        {
            IReadOnlyList<Page> navigationStack = _navigation.Navigation.NavigationStack;
            if (navigationStack.Count == 1)
            {
                _navStack.Pop();
                if (_navStack.Count > 1)
                    await NavigateTo(_navStack.Pop());
                else
                    await _navigation.PopAsync();
                return;
            }

            while (numberOfPagesToSkip > 0)
            {
                _navigation.Navigation.RemovePage(navigationStack[navigationStack.Count - 2]);
                numberOfPagesToSkip--;
            }

            await _navigation.PopAsync();
        }

        public async Task PopToRoot()
        {
            await _navigation.PopToRootAsync(true);
        }

        private ContentPage GetPage(PageKey pageKey)
        {
            ContentPage page;

            switch (pageKey)
            {
                case PageKey.LoginPage:
                    page = ContainerManager.Container.Resolve<LoginPage>();
                    break;
                case PageKey.MenuPage:
                    page = ContainerManager.Container.Resolve<MenuPage>();
                    break;
                default:
                    throw new ArgumentException($"No such page: {pageKey}. Did you forget to call NavigationService.Configure?", nameof(pageKey));
            }

            if (!_pages.ContainsKey(page.GetType()))
                _pages.Add(page.GetType(), pageKey);

            return page;
        }
    }
}