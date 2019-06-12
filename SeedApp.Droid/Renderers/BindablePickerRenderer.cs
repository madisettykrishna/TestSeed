using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Util;
using Android.Views;
using Android.Widget;
using SeedApp.Controls;
using SeedApp.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(BindablePicker), typeof(BindablePickerRenderer))]

namespace SeedApp.Droid.Renderers
{
    public class BindablePickerRenderer : PickerRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);
            Control?.SetBackgroundColor(Android.Graphics.Color.Transparent);
            var nativeEditText = (global::Android.Widget.EditText)Control;

            if (nativeEditText != null && e.NewElement is BindablePicker && !string.IsNullOrEmpty((e.NewElement as BindablePicker).LeftImageSource))
            {
                nativeEditText.SetBackgroundResource(Resource.Drawable.bg_edittext_icon);
                nativeEditText.SetCompoundDrawablesWithIntrinsicBounds(DrawableResourceId((e.NewElement as BindablePicker).LeftImageSource), 0, 0, 0);
                nativeEditText.CompoundDrawablePadding = 15;
            }
            else
            {
                nativeEditText.SetBackgroundResource(Resource.Drawable.bg_edittext_border);
            }
        }

        //// <summary>
        //// Get the ResourceId of an image from the file name
        //// </summary>
        //// <returns>The resource identifier.</returns>
        //// <param name="name">Name.</param>
        public int DrawableResourceId(string name)
        {
            if (name.Contains(".png"))
            {
                name = name.Substring(0, name.LastIndexOf(".png"));
            }
            return Resources.GetIdentifier(name, "drawable", Context.PackageName);
        }
    }

    public class PickerRenderer : ViewRenderer<Picker, EditText>
    {
        AlertDialog _dialog;
        bool _isDisposed;

        public PickerRenderer()
        {
            AutoPackage = false;
        }

        IElementController ElementController => Element as IElementController;

        protected override void Dispose(bool disposing)
        {
            if (disposing && !_isDisposed)
            {
                _isDisposed = true;
                ((INotifyCollectionChanged)Element.Items).CollectionChanged -= RowsCollectionChanged;
            }

            base.Dispose(disposing);
        }

        protected override EditText CreateNativeControl()
        {
            return new EditText(Context) { Focusable = false, Clickable = true, Tag = this };
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            if (e.OldElement != null)
                ((INotifyCollectionChanged)e.OldElement.Items).CollectionChanged -= RowsCollectionChanged;

            if (e.NewElement != null)
            {
                ((INotifyCollectionChanged)e.NewElement.Items).CollectionChanged += RowsCollectionChanged;
                if (Control == null)
                {
                    var textField = CreateNativeControl();
                    textField.SetOnClickListener(PickerListener.Instance);
                    SetNativeControl(textField);
                }

                UpdatePicker();
                UpdateTextColor();
            }

            base.OnElementChanged(e);
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == Picker.TitleProperty.PropertyName)
                UpdatePicker();
            if (e.PropertyName == Picker.SelectedIndexProperty.PropertyName)
                UpdatePicker();
            if (e.PropertyName == Picker.TextColorProperty.PropertyName)
                UpdateTextColor();
        }

        private void OnClick()
        {
            Picker model = Element;

            var picker = new CustomNumberPicker(Context);
            if (model.Items != null && model.Items.Any())
            {
                picker.MaxValue = model.Items.Count - 1;
                picker.MinValue = 0;
                picker.SetDisplayedValues(model.Items.ToArray());
                picker.WrapSelectorWheel = false;
                picker.DescendantFocusability = Android.Views.DescendantFocusability.BlockDescendants;
                picker.Value = model.SelectedIndex;
            }

            var layout = new LinearLayout(Context) { Orientation = Android.Widget.Orientation.Vertical };
            layout.AddView(picker);

            ElementController.SetValueFromRenderer(VisualElement.IsFocusedProperty, true);

            LinearLayout.LayoutParams paramslnr = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.WrapContent);
            LinearLayout LLayout = new LinearLayout(Context);
            LLayout.Orientation = Android.Widget.Orientation.Vertical;
            paramslnr.SetMargins(15, DpToPixels(10), 15, 15);
            LLayout.LayoutParameters = paramslnr;
            LLayout.SetPadding(15, 15, 15, 15);
            TextView tv_title = new TextView(Context);
            tv_title.LayoutParameters = paramslnr;
            tv_title.SetTypeface(tv_title.Typeface, TypefaceStyle.Bold);
            tv_title.SetTextColor(Android.Graphics.Color.Black);
            tv_title.SetTextSize(ComplexUnitType.Dip, 20);
            tv_title.Gravity = (GravityFlags.Center);
            tv_title.Text = model.Title ?? string.Empty;

            //// Add the two views to linear layout
            LLayout.AddView(tv_title);

            var builder = new AlertDialog.Builder(Context);
            builder.SetView(layout);
            ////builder.SetTitle(model.Title ?? "");
            builder.SetCustomTitle(LLayout);
            builder.SetNegativeButton(global::Android.Resource.String.Cancel, (s, a) =>
            {
                ElementController.SetValueFromRenderer(VisualElement.IsFocusedProperty, false);
                //// It is possible for the Content of the Page to be changed when Focus is changed.
                //// In this case, we'll lose our Control.
                Element.SelectedItem = null;
                Control.Text = null;
                Control?.ClearFocus();
                _dialog = null;
            });

            builder.SetPositiveButton(global::Android.Resource.String.Ok, (s, a) =>
            {
                ElementController.SetValueFromRenderer(Picker.SelectedIndexProperty, picker.Value);
                //// It is possible for the Content of the Page to be changed on SelectedIndexChanged. 
                //// In this case, the Element & Control will no longer exist.
                if (Element != null)
                {
                    if (model.Items.Count > 0 && Element.SelectedIndex >= 0)
                        Control.Text = model.Items[Element.SelectedIndex];
                    ElementController.SetValueFromRenderer(VisualElement.IsFocusedProperty, false);
                    //// It is also possible for the Content of the Page to be changed when Focus is changed.
                    //// In this case, we'll lose our Control.
                    Control?.ClearFocus();
                }
                _dialog = null;
            });

            _dialog = builder.Create();
            _dialog.DismissEvent += (sender, args) =>
            {
                ElementController.SetValueFromRenderer(VisualElement.IsFocusedProperty, false);
            };
            _dialog.Show();
        }

        public int DpToPixels(int dp)
        {
            return (int)(dp * Android.App.Application.Context.Resources.DisplayMetrics.Density);
        }

        void RowsCollectionChanged(object sender, EventArgs e)
        {
            UpdatePicker();
        }

        void UpdatePicker()
        {
            Control.Hint = Element.Title;

            string oldText = Control.Text;

            if (Element.SelectedIndex == -1 || Element.Items == null)
                Control.Text = null;
            else
                Control.Text = Element.Items[Element.SelectedIndex];

            if (oldText != Control.Text)
                ((IVisualElementController)Element).NativeSizeChanged();
        }

        void UpdateTextColor()
        {
            if (Control != null)
            {
                Control.SetTextColor(Element.TextColor.ToAndroid());
            }
        }

        class PickerListener : Java.Lang.Object, IOnClickListener
        {
            public static readonly PickerListener Instance = new PickerListener();

            public void OnClick(global::Android.Views.View v)
            {
                var renderer = v.Tag as PickerRenderer;
                if (renderer == null)
                    return;

                renderer.OnClick();
            }
        }

        public class CustomNumberPicker : Android.Widget.NumberPicker
        {
            public CustomNumberPicker(Context context) : base(context)
            {
            }

            public CustomNumberPicker(Context context, IAttributeSet attrs) : base(context, attrs)
            {
            }

            public override void AddView(Android.Views.View child)
            {
                base.AddView(child);
                updateView(child);
            }

            public override void AddView(Android.Views.View child, Android.Views.ViewGroup.LayoutParams @params)
            {
                base.AddView(child, @params);
                updateView(child);
            }

            public override void AddView(Android.Views.View child, int index, Android.Views.ViewGroup.LayoutParams @params)
            {
                base.AddView(child, index, @params);
                updateView(child);
            }

            private void updateView(Android.Views.View view)
            {
                if (view is EditText)
                {
                    ((EditText)view).SetTextSize(Android.Util.ComplexUnitType.Sp, 17);
                }
            }
        }
    }
}
