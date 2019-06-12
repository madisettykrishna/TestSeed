using System;
using System.Collections.Generic;
using SeedApp.Cells;
using SeedApp.ViewModels;
using Xamarin.Forms;

namespace SeedApp.Views
{
    public partial class MenuPage : ContentPage
    {
        private readonly MenuPageViewModel _menuPageViewModel;

        public MenuPage(MenuPageViewModel menuPageViewModel)
        {
            InitializeComponent();
            _menuPageViewModel = menuPageViewModel;
            BindingContext = _menuPageViewModel;
            Title = "SeedApp";
            BackgroundColor = Color.Black;
            FileImageSource slideIcon = new FileImageSource { File = "ico_slide_menu.png" };
            Icon = slideIcon;

            MenuListView.ItemTemplate = new DataTemplate(typeof(MenuItemCell));
            MenuListView.ItemSelected += (object sender, SelectedItemChangedEventArgs e) => { MenuListView.SelectedItem = null; };
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            try
            {
                await _menuPageViewModel.OnViewAppear();

                (App.Current.MainPage as MasterDetailPage).IsPresentedChanged += (object sender, EventArgs e) =>
                {
                    DependencyService.Get<IKeyboardInteractions>().HideKeyboard();
                };
            }
            catch
            {
            }
        }
    }
}
