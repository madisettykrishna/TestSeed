using Android.Text;
using Android.Widget;
using SeedApp.Controls;
using SeedApp.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(HtmlLabel), typeof(HtmlLabelRenderer))]

namespace SeedApp.Droid.Renderers
{
    public class HtmlLabelRenderer : LabelRenderer
    {
        public HtmlLabelRenderer(MainActivity context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);
            if (Element != null && !string.IsNullOrEmpty(Element.Text))
            {
                Initialize();
            }
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == Label.TextProperty.PropertyName)
            {
                Initialize();
            }
        }

        public void Initialize()
        {
            if (Control != null)
            {
                var view = (HtmlLabel)Element;
                if (view == null) return;

                Control?.SetMaxLines(int.MaxValue);
                Control.SetText(Html.FromHtml(view.Text.ToString()), TextView.BufferType.Spannable);
            }
        }
    }
}
