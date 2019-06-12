using SeedApp.Controls;
using SeedApp.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CommonEditor), typeof(CommonEditorRenderer))]

namespace SeedApp.iOS.Renderers
{
    public class CommonEditorRenderer : EditorRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);

            if (Control != null && (e.NewElement is CommonEditor) && (e.NewElement as CommonEditor).IsBorder)
            {
                Control.Layer.BorderWidth = 1f;
                Control.Layer.BorderColor = UIColor.FromRGB(230, 230, 230).CGColor;
            }

            if (Control != null && (e.NewElement is CommonEditor) && (e.NewElement as CommonEditor).IsCustomHeight)
            {
                Control.ScrollEnabled = false;
            }
        }
    }
}
