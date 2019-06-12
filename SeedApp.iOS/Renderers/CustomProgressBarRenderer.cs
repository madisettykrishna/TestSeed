using SeedApp.Controls;
using SeedApp.iOS.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomProgressBar), typeof(CustomProgressBarRenderer))]

namespace SeedApp.iOS.Renderers
{
    public class CustomProgressBarRenderer : ProgressBarRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<ProgressBar> e)
        {
            base.OnElementChanged(e);
            Control.ProgressTintColor = Color.FromRgb(51, 0, 102).ToUIColor();
        }
    }
}
