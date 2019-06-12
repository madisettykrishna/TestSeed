using System;
using SeedApp.Controls;
using SeedApp.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CommonEditor), typeof(CommonEditorRenderer))]

namespace SeedApp.Droid.Renderers
{
    public class CommonEditorRenderer : EditorRenderer
    {
        public CommonEditorRenderer(MainActivity context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);
            if (Element != null)
            {
                Control.SetBackgroundColor(Android.Graphics.Color.Transparent);

                if (e.NewElement is CommonEditor && (e.NewElement as CommonEditor).IsBorder)
                {
                    Control.SetBackgroundResource(Resource.Drawable.bg_edittext_border);
                }

                if (Control != null && (e.NewElement is CommonEditor) && (e.NewElement as CommonEditor).IsCustomHeight)
                {
                    Control.VerticalScrollBarEnabled = false;
                }
            }
        }
    }
}
