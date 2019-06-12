using SeedApp.Controls;
using SeedApp.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomDateEntry), typeof(CustomDateTimePickerRenderer))]

namespace SeedApp.Droid.Renderers
{
    public class CustomDateTimePickerRenderer : EntryRenderer
    {
        public CustomDateTimePickerRenderer(MainActivity context) : base(context)
        {
        }
    }
}
