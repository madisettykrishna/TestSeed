using Xamarin.Forms;

namespace SeedApp.Controls
{
    public class CommonEntry : Entry
    {
        public static readonly BindableProperty IsBorderProperty =
            BindableProperty.Create<CommonEntry, bool>(o => o.IsBorder, false);

        public static readonly BindableProperty LeftImageSourceProperty =
            BindableProperty.Create<CommonEntry, string>(o => o.LeftImageSource, string.Empty);
    
        public string LeftImageSource
        {
            get { return (string)GetValue(LeftImageSourceProperty); }
            set { SetValue(LeftImageSourceProperty, value); }
        }

        public bool IsBorder
        {
            get { return (bool)GetValue(IsBorderProperty); }
            set { SetValue(IsBorderProperty, value); }
        }
    }
}