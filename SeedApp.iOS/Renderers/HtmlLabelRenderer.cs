using System;
using System.ComponentModel;
using Foundation;
using SeedApp.Controls;
using SeedApp.iOS.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(HtmlLabel), typeof(HtmlLabelRenderer))]

namespace SeedApp.iOS.Renderers
{
    public class HtmlLabelRenderer : LabelRenderer
    {
        public void Initialize()
        {
            if (Control != null && Element != null && !string.IsNullOrWhiteSpace(Element.Text))
            {
                var view = (HtmlLabel)Element;
                if (view == null) return;

                ////Original Credits : https://forums.xamarin.com/discussion/23670/how-to-display-html-formatted-text-in-a-uilabel
                var attr = new NSAttributedStringDocumentAttributes();
                var nsError = new NSError();
                attr.DocumentType = NSDocumentType.HTML;

                Control.AttributedText = new NSAttributedString(view.Text, attr, ref nsError);
                Control.UserInteractionEnabled = false;
            }
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);
            Initialize();
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == Label.TextProperty.PropertyName)
            {
                Initialize();
            }
        }
    }
}
