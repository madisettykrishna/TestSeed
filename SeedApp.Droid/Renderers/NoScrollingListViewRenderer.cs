using SeedApp.Controls;
using SeedApp.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(NoScrollingListView), typeof(NoScrollingListViewRenderer))]

namespace SeedApp.Droid.Renderers
{
    public class NoScrollingListViewRenderer : ListViewRenderer
    {
        public NoScrollingListViewRenderer(MainActivity context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<ListView> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || this.Element == null)
            {
                return;
            }

            Control.SmoothScrollbarEnabled = false;
            Control.FastScrollEnabled = false;
            Control.SetBackgroundColor(Android.Graphics.Color.White);
        }
    }
}