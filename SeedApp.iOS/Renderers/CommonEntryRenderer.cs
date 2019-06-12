using System.ComponentModel;
using CoreGraphics;
using SeedApp.Controls;
using SeedApp.IOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CommonEntry), typeof(CommonEntryRenderer))]

namespace SeedApp.IOS.Renderers
{
    public class CommonEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement is CommonEntry)
            {
                InitializeNativeControl(e.NewElement as CommonEntry);
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == Entry.PlaceholderProperty.PropertyName)
            {
                Control.AttributedPlaceholder?.Dispose();
            }
        }

        private void InitializeNativeControl(CommonEntry entry)
        {
            Control.BorderStyle = UITextBorderStyle.None;

            UIView controlLeftView = new UIView(new CGRect(0, 0, 8, Frame.Height));

            if (!string.IsNullOrEmpty(entry.LeftImageSource))
            {
                controlLeftView.Frame = new CGRect(0, 0, 50, Frame.Height);
                UIView leftView = new UIView(new CGRect(0, 0, Frame.Height + 5, Frame.Height + 5));
                leftView.BackgroundColor = UIColor.FromRGB(238, 238, 238);

                UIImageView imageView = new UIImageView();
                imageView.Frame = new CGRect(10, 10, 25, 25);
                imageView.Image = UIImage.FromFile(entry.LeftImageSource);
                imageView.ContentMode = UIViewContentMode.ScaleAspectFit;
                leftView.Add(imageView);

                UIView leftLineView = new UIView(new CGRect(leftView.Frame.Width - 1, 0, 1, leftView.Frame.Height));
                leftLineView.BackgroundColor = UIColor.FromRGB(230, 230, 230);
                leftView.Add(leftLineView);

                Control.Add(leftView);
            }

            UIView rightView = new UIView(new CGRect(0, Frame.Width - 8, 8, Frame.Height));

            Control.LeftView = controlLeftView;
            Control.LeftViewMode = UITextFieldViewMode.Always;
            Control.RightView = rightView;
            Control.RightViewMode = UITextFieldViewMode.UnlessEditing;
            Control.TextColor = UIColor.FromRGB(51, 0, 102);

            if (entry.IsBorder)
            {
                Control.Layer.BorderWidth = 1f;
                Control.Layer.BorderColor = UIColor.FromRGB(230, 230, 230).CGColor;
            }
        }
    }
}