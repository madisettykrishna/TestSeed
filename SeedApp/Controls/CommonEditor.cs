using System;
using Xamarin.Forms;

namespace SeedApp.Controls
{
    public class CommonEditor : Editor
    {
        public static readonly BindableProperty IsCustomHeightProperty = BindableProperty.Create<CommonEditor, bool>(o => o.IsCustomHeight, false);

        public static readonly BindableProperty IsBorderProperty = BindableProperty.Create<CommonEditor, bool>(o => o.IsBorder, false);

        public CommonEditor()
        {
            this.TextChanged += OnTextChanged_Grow;
        }

        public bool IsBorder
        {
            get { return (bool)GetValue(IsBorderProperty); }
            set { SetValue(IsBorderProperty, value); }
        }

        public bool IsCustomHeight
        {
            get { return (bool)GetValue(IsCustomHeightProperty); }
            set { SetValue(IsCustomHeightProperty, value); }
        }

        private void OnTextChanged_Grow(Object sender, TextChangedEventArgs e)
        {
            if (IsCustomHeight)
            {
                this.InvalidateMeasure();
            }
        }
    }
}
