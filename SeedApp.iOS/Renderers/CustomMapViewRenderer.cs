using System;
using MapKit;
using SeedApp.Controls;
using SeedApp.iOS.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Maps.iOS;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomMapView), typeof(CustomMapViewRenderer))]

namespace SeedApp.iOS.Renderers
{
    public class CustomMapViewRenderer : MapRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<View> e)
        {
            base.OnElementChanged(e);

            var nativeMap = Control as MKMapView;
            nativeMap.ShowsUserLocation = true;
            var x = nativeMap.UserLocation;
        }
    }
}
