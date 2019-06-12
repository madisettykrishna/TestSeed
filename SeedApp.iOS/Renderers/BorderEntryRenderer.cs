using System;
using System.ComponentModel;
using CoreGraphics;
using SeedApp.Controls;
using SeedApp.IOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(BorderEntry), typeof(BorderEntryRenderer))]

namespace SeedApp.IOS.Renderers
{
    public class BorderEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            InitializeNativeControl();
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == Entry.PlaceholderProperty.PropertyName)
            {
                Control.AttributedPlaceholder?.Dispose();
            }
        }

        private void InitializeNativeControl()
        {
            Control.BorderStyle = UITextBorderStyle.None;

            UIView leftView = new UIView(new CGRect(0, 0, 8, Frame.Height));
            UIView rightView = new UIView(new CGRect(0, Frame.Width - 8, 8, Frame.Height));

            Control.LeftView = leftView;
            Control.LeftViewMode = UITextFieldViewMode.Always;
            Control.RightView = rightView;
            Control.RightViewMode = UITextFieldViewMode.UnlessEditing;

            Control.TextColor = UIColor.Black;

            Control.Layer.BorderWidth = 1f;
            Control.Layer.CornerRadius = 5f;
            Control.Layer.BorderColor = UIColor.FromRGB(230, 230, 230).CGColor;
        }
    }
}
