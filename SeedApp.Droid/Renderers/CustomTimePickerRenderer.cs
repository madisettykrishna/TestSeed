using System;
using Android.App;
using Android.Runtime;
using SeedApp.Controls;
using SeedApp.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomTimePicker), typeof(CustomTimePickerRenderer))]

namespace SeedApp.Droid.Renderers
{
    public class CustomTimePickerRenderer : ViewRenderer<Xamarin.Forms.TimePicker, Android.Widget.EditText>, TimePickerDialog.IOnTimeSetListener, IJavaObject, IDisposable
    {
        private TimePickerDialog dialog = null;
        private CustomTimePicker _picker;

        public CustomTimePickerRenderer(MainActivity context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.TimePicker> e)
        {
            base.OnElementChanged(e);
            try
            {
                _picker = Element as CustomTimePicker;

                this.SetNativeControl(new Android.Widget.EditText(Forms.Context));

                this.Control.Click += Control_Click;
                this.Control.Text = DateTime.UtcNow.ToLocalTime().ToString("hh:mm tt");
                this.Control.KeyListener = null;
                this.Control.FocusChange += Control_FocusChange;
            }
            catch (Exception ex)
            {
            }
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            try
            {
                if (e.PropertyName == CustomTimePicker.TimeProperty.PropertyName)
                {
                    Control.Text = (DateTime.Today + _picker.Time).ToString("hh:mm tt");
                }
            }
            catch (Exception ex)
            {
            }
        }

        void Control_FocusChange(object sender, Android.Views.View.FocusChangeEventArgs e)
        {
            if (e.HasFocus)
                ShowTimePicker();
        }

        void Control_Click(object sender, EventArgs e)
        {
            ShowTimePicker();
        }

        private void ShowTimePicker()
        {
            try
            {
                if (dialog == null)
                {
                    dialog = new TimePickerDialog(Forms.Context, this, DateTime.Now.Hour, DateTime.Now.Minute, false);
                }
                dialog.Show();
            }
            catch (Exception ex)
            {
            }
        }

        public void OnTimeSet(Android.Widget.TimePicker view, int hourOfDay, int minute)
        {
            try
            {
                var time = new TimeSpan(hourOfDay, minute, 0);
                DateTime dt = DateTime.Today + time;
                this.Element.SetValue(Xamarin.Forms.TimePicker.TimeProperty, time);
                this.Control.Text = dt.ToLocalTime().ToString("hh:mm tt");
            }
            catch (Exception ex)
            {
            }
        }
    }
}
