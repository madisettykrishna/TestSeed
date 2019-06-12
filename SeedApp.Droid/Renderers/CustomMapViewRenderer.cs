using SeedApp.Controls;
using SeedApp.Droid;
using Micronet.MemberZoneStaff.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.Android;

[assembly: ExportRenderer(typeof(CustomMapView), typeof(CustomMapViewRenderer))]

namespace Micronet.MemberZoneStaff.Droid.Renderers
{
    public class CustomMapViewRenderer : MapRenderer
    {
        public CustomMapViewRenderer(MainActivity context) : base(context)
        {
        }

        protected override void OnElementChanged(Xamarin.Forms.Platform.Android.ElementChangedEventArgs<Map> e)
        {
            base.OnElementChanged(e);
        }
    }
}