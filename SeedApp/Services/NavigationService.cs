using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SeedApp.Common;
using SeedApp.Common.Constants;
using SeedApp.Common.Enums;
using SeedApp.Common.Interfaces;
using SeedApp.Forms.Views;
using Xamarin.Forms;

namespace SeedApp.Forms.Services
{
	public class NavigationService : INavigationService
	{
		private readonly Dictionary<Type, PageKey> _pages;

		private MasterDetailPage _masterDetailPage;
		private NavigationPage _navigation;
		private MenuPage _menuPage;

		public NavigationService()
		{
			_pages = new Dictionary<Type, PageKey>();
		}

		public PageKey CurrentPageKey => _pages[_navigation.CurrentPage.GetType()];

		public async Task NavigateToFirstScreenAsync()
		{
			await NavigateToAsync(PageKey.FirstPage);
		}

		public async Task NavigateToAsync(PageKey pageKey, bool isAnimated = false)
		{
			ContentPage page = GetPage(pageKey);
			if (pageKey == PageKey.FirstPage)
			{
				if (_masterDetailPage == null)
				{
					_masterDetailPage = new MasterDetailPage();

					_menuPage = (MenuPage)ContainerManager.Container.Resolve(typeof(MenuPage),typeof(MenuPage).GetType().ToString());
				}

				_masterDetailPage.Master = new NavigationPage(_menuPage)
				{
					Title = "Menu",
					BarTextColor = Color.White
				};

				_masterDetailPage.Detail = _navigation = new NavigationPage(page)
				{
					BackgroundColor = ColorConstants.DarkAppColor,
					BarBackgroundColor = ColorConstants.DarkAppColor,
					BarTextColor = Color.White
				};

				App.Current.MainPage = _masterDetailPage;
				_masterDetailPage.IsPresented = false;
			}
			else
			{
				if (_masterDetailPage != null)
				{
					_masterDetailPage.IsPresented = false;
				}

				await _navigation.PushAsync(page, isAnimated);
			}
		}

		public async Task GoBackAsync(int numberOfPagesToSkip = 0)
		{
			IReadOnlyList<Page> navigationStack = _navigation.Navigation.NavigationStack;
			while (numberOfPagesToSkip > 0)
			{
				_navigation.Navigation.RemovePage(navigationStack[navigationStack.Count - 2]);
				numberOfPagesToSkip--;
			}

			await _navigation.PopAsync();
		}

		public async Task GoBackToPageAsync(PageKey pageKey)
		{
			IReadOnlyList<Page> navigationStack = _navigation.Navigation.NavigationStack;

			var numberOfPagesToSkip = 0;

			if (navigationStack.Count > 2)
			{
				var startIndex = navigationStack.Count - 2;

				for (var i = startIndex; i >= 0; i--)
				{
					var page = navigationStack[i];

					var pageType = _pages.First(x => x.Value == pageKey).Key;

					if (page.GetType() == pageType)
					{
						numberOfPagesToSkip = startIndex - i;
						break;
					}
				}
			}

			await GoBackAsync(numberOfPagesToSkip);
		}

		public async Task PopToRootAsync()
		{
			await _navigation.PopToRootAsync(true);
		}

		public void PresentMenuView()
		{
			if (_masterDetailPage != null)
			{
				_masterDetailPage.IsPresented = !_masterDetailPage.IsPresented;
			}
		}


		private ContentPage GetPage(PageKey pageKey)
		{
			ContentPage page;
			switch (pageKey)
			{
				case PageKey.FirstPage:
					page = (ContentPage)ContainerManager.Container.Resolve(typeof(FirstPage), typeof(FirstPage).GetType().ToString());
					break;
				default:
					throw new ArgumentException(
						$"No such page: {pageKey}. Did you forget to call NavigationService.Configure?", nameof(pageKey));
			}

			if (!_pages.ContainsKey(page.GetType()))
				_pages.Add(page.GetType(), pageKey);

			return page;
		}
	}
}
