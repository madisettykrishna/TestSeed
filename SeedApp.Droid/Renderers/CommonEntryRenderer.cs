using SeedApp.Controls;
using SeedApp.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CommonEntry), typeof(CommonEntryRenderer))]

namespace SeedApp.Droid.Renderers
{
    public class CommonEntryRenderer : EntryRenderer
    {
        public CommonEntryRenderer(MainActivity context) : base(context)
        {
        }

        public int DrawableResourceId(string name)
        {
            if (name.Contains(".png"))
            {
                name = name.Substring(0, name.LastIndexOf(".png"));
            }

            return Resources.GetIdentifier(name, "drawable", Context.PackageName);
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Element != null)
            {
                if (Control != null)
                {
                    Control.SetBackgroundColor(Android.Graphics.Color.Transparent);
                }

                var nativeEditText = (global::Android.Widget.EditText)Control;

                if (nativeEditText != null)
                {
                    if (e.NewElement is CommonEntry && (e.NewElement as CommonEntry).IsBorder)
                    {
                        nativeEditText.SetBackgroundResource(Resource.Drawable.bg_edittext_icon);
                    }

                    if (e.NewElement is CommonEntry && !string.IsNullOrEmpty((e.NewElement as CommonEntry).LeftImageSource))
                    {
                        nativeEditText.SetBackgroundResource(Resource.Drawable.bg_edittext_icon);
                        nativeEditText.SetCompoundDrawablesWithIntrinsicBounds(DrawableResourceId((e.NewElement as CommonEntry).LeftImageSource), 0, 0, 0);
                        nativeEditText.CompoundDrawablePadding = 15;
                    }
                }
            }
        }
    }
}