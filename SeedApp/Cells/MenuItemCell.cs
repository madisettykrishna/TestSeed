using System.Threading.Tasks;
using SeedApp.Models;
using Xamarin.Forms;

namespace SeedApp.Cells
{
    public class MenuItemCell : ViewCell
    {
        public const double CellHeight = 55;
        private StackLayout _stackLayout;
        private Label _lblTitle;
        private Image _imgIcon;

        public MenuItemCell()
        {
            Height = CellHeight;

            _lblTitle = new Label()
            {
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalTextAlignment = TextAlignment.Start,
                TextColor = Color.White,
                FontFamily = "Roboto",
                FontSize = 16
            };

            _imgIcon = new Image
            {
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Center,
                HeightRequest = 30,
                WidthRequest = 30,
                Aspect = Aspect.AspectFit
            };

            _stackLayout = new StackLayout()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Padding = new Thickness(20, 10, 25, 10),
                Spacing = 10,
                Orientation = StackOrientation.Horizontal,
                Children = { _imgIcon, _lblTitle }
            };

            View = _stackLayout;
            _imgIcon.SetBinding<MenuInfo>(Image.SourceProperty, (s) => s.Icon);
            _lblTitle.SetBinding<MenuInfo>(Label.TextProperty, (s) => s.Title);

            ////this.Tapped += (sender, e) => 
            ////{
            ////    Task.Run(async () => 
            ////    {    
            ////        Device.BeginInvokeOnMainThread(() => this.View.BackgroundColor = new Color(230, 230, 230));
            ////    });
            ////};
        }
    }
}