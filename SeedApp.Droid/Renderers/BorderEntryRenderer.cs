using Android.Graphics;
using Android.Graphics.Drawables;
using SeedApp.Controls;
using SeedApp.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(BorderEntry), typeof(BorderEntryRenderer))]

namespace SeedApp.Droid.Renderers
{
    public class BorderEntryRenderer : EntryRenderer
    {
        public BorderEntryRenderer(MainActivity context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if (Element != null)
            {
                Control.SetBackgroundColor(Android.Graphics.Color.Transparent);
                var nativeEditText = (global::Android.Widget.EditText)Control;
                var shape = new ShapeDrawable(new Android.Graphics.Drawables.Shapes.RectShape());
                shape.Paint.Color = Xamarin.Forms.Color.Gray.ToAndroid();
                shape.Paint.SetStyle(Paint.Style.Stroke);
                nativeEditText.Background = shape;
            }
        }
    }
}
