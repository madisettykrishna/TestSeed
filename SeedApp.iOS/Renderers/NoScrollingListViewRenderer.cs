using SeedApp.Controls;
using SeedApp.IOS.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(NoScrollingListView), typeof(NoScrollingListViewRenderer))]

namespace SeedApp.IOS.Renderers
{
    public class NoScrollingListViewRenderer : ListViewRenderer
    {
        public NoScrollingListViewRenderer()
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<ListView> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || this.Element == null)
            {
                return;
            }

            Control.ScrollEnabled = false;
        }
    }
}