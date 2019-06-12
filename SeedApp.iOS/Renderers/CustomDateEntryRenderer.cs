using System;
using CoreGraphics;
using Foundation;
using SeedApp.Controls;
using SeedApp.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomDateEntry), typeof(CustomDateEntryRenderer))]

namespace SeedApp.iOS.Renderers
{
    public class CustomDateEntryRenderer : EntryRenderer
    {
        private UIDatePicker _datePicker;
        private CustomDateEntry _customDateEntry;
        private DateTime _selectedDateTime;

        public CustomDateEntryRenderer()
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            try
            {
                if (Element != null && _datePicker == null)
                {
                    _datePicker = new UIDatePicker();
                    _customDateEntry = (CustomDateEntry)Element;
                    _datePicker.TimeZone = NSTimeZone.LocalTimeZone;
                    var dobToolBar = new UIToolbar(new CGRect(0, 0, UIScreen.MainScreen.Bounds.Width, 50));
                    dobToolBar.BarStyle = UIBarStyle.Default;
                    dobToolBar.Items = new UIBarButtonItem[]
                    {
                    new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace),
                    new UIBarButtonItem(UIBarButtonSystemItem.Done, (sender, args) => Control.ResignFirstResponder())
                    };

                    dobToolBar.SizeToFit();
                    Control.ShouldChangeCharacters += (textField, range, replacementString) => false;
                    Control.InputAccessoryView = dobToolBar;
                    Control.BorderStyle = UITextBorderStyle.None;
                    Control.InputView = _datePicker;
                    _selectedDateTime = _customDateEntry.SelectedDateTime;
                    _datePicker.Date = ConvertDateTimeToNSDate(_selectedDateTime);
                    UpdatePicker();
                    UpdateCustomDateEntry();
                }

                if (e.NewElement != null)
                {
                    _datePicker.ValueChanged += DateTimeSelected;
                }

                if (e.OldElement != null)
                {
                    _datePicker.ValueChanged -= DateTimeSelected;
                }
            }
            catch
            {
            }
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            try
            {
                if (e.PropertyName == CustomDateEntry.AllDayEventEnabledProperty.PropertyName ||
                    e.PropertyName == CustomDateEntry.SelectedDateTimeProperty.PropertyName)
                {
                    UpdatePicker();
                    _selectedDateTime = _customDateEntry.SelectedDateTime;
                    _datePicker.Date = ConvertDateTimeToNSDate(_selectedDateTime);
                    UpdateCustomDateEntry();
                }
            }
            catch
            {
            }
        }

        private void DateTimeSelected(object sender, EventArgs args)
        {
            try
            {
                _selectedDateTime = ConvertNsDateToDateTime(_datePicker.Date).ToLocalTime();
                UpdateCustomDateEntry();
                _customDateEntry.SetValue(CustomDateEntry.SelectedDateTimeProperty, _selectedDateTime);
            }
            catch
            {
            }
        }

        private void UpdatePicker()
        {
            _datePicker.Mode = _customDateEntry.AllDayEventEnabled ? UIDatePickerMode.Date : UIDatePickerMode.DateAndTime;
        }

        private void UpdateCustomDateEntry()
        {
            Control.Text = _customDateEntry.AllDayEventEnabled ? _selectedDateTime.ToString("MM/dd/yyyy") : _selectedDateTime.ToString("MM/dd/yyyy hh:mm tt") + "  ";
        }

        private DateTime ConvertNsDateToDateTime(NSDate date)
        {
            DateTime newDate = TimeZone.CurrentTimeZone.ToLocalTime(
                new DateTime(2001, 1, 1, 0, 0, 0));
            return newDate.AddSeconds(date.SecondsSinceReferenceDate);
        }

        private NSDate ConvertDateTimeToNSDate(DateTime date)
        {
            DateTime newDate = TimeZone.CurrentTimeZone.ToLocalTime(
                new DateTime(2001, 1, 1, 0, 0, 0));
            return NSDate.FromTimeIntervalSinceReferenceDate(
                (date - newDate).TotalSeconds);
        }
    }
}
