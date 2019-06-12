using SeedApp.Controls;
using SeedApp.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRendererAttribute(typeof(FrameButton), typeof(FrameButtonRenderer))]

namespace SeedApp.Droid.Renderers
{
    public class FrameButtonRenderer : ButtonRenderer
    {
        public FrameButtonRenderer(MainActivity context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            try
            {
                base.OnElementChanged(e);
                this.Control.SetAllCaps(false);
            }
            catch
            {
            }
        }
    }
}
