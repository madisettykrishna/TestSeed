using System;
using System.ComponentModel;
using Xamarin.Forms;

namespace SeedApp.Controls
{
    public class DateTimePicker : StackLayout
    {
        public static readonly BindableProperty IsAllDayEventProperty = BindableProperty.Create("IsAllDay", typeof(bool), typeof(DateTimePicker), false, BindingMode.Default, null, (bindable, oldValue, newValue) =>
        {
            var element = bindable as DateTimePicker;
            element.ChangeTimeVisibility(!element.IsAllDayEvent);
        });

        public static readonly BindableProperty DateTimeProperty =
            BindableProperty.Create("DateTime", typeof(DateTime), typeof(DateTimePicker), DateTime.Now, BindingMode.Default, null, (bindable, oldValue, newValue) =>
            {
                (bindable as DateTimePicker).SetDateTime((DateTime)newValue);
            });

        private readonly DatePicker _date;
        private readonly CustomTimePicker _time;
        private DateTime _dateTime;

        public DateTimePicker()
        {
            this.Orientation = StackOrientation.Horizontal;
            this.HorizontalOptions = LayoutOptions.EndAndExpand;
            this.Padding = 2;
            _date = new DatePicker();
            _time = new CustomTimePicker();
            this.Children.Add(_date);
            this.Children.Add(_time);
            _date.PropertyChanged += DateOnPropertyChanged;
            _time.PropertyChanged += TimeOnPropertyChanged;
            _dateTime = DateTime.Now;
        }

        public bool IsAllDayEvent
        {
            get { return (bool)GetValue(IsAllDayEventProperty); }
            set { SetValue(IsAllDayEventProperty, value); }
        }

        public DateTime DateTime
        {
            get { return (DateTime)GetValue(DateTimeProperty); }
            set { SetValue(DateTimeProperty, value); }
        }

        public void ChangeTimeVisibility(bool isVisible)
        {
            _time.IsVisible = isVisible;
        }

        public void SetDateTime(DateTime dateTime)
        {
            try
            {
                if (dateTime.Year <= 1900 && _dateTime.Year <= 1900)
                    return;

                if (dateTime.Year <= 1900)
                {
                    dateTime = _dateTime;
                }

                if (AreDifferent(dateTime, _dateTime, true))
                {
                    _time.Time = new TimeSpan(dateTime.Hour, dateTime.Minute, 0);
                }

                if (AreDifferent(dateTime, _dateTime))
                {
                    _date.Date = dateTime;
                }

                _dateTime = new DateTime(_date.Date.Year, _date.Date.Month, _date.Date.Day, _time.Time.Hours, _time.Time.Minutes, 0);
            }
            catch (Exception)
            {
            }
        }

        private void TimeOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            try
            {
                _dateTime = new DateTime(_date.Date.Year, _date.Date.Month, _date.Date.Day, _time.Time.Hours, _time.Time.Minutes, 0);
                DateTime = _dateTime;
            }
            catch (Exception)
            {
            }
        }

        private void DateOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            try
            {
                _dateTime = new DateTime(_date.Date.Year, _date.Date.Month, _date.Date.Day, _time.Time.Hours, _time.Time.Minutes, 0);
                DateTime = _dateTime;
            }
            catch (Exception)
            {
            }
        }

        private bool AreDifferent(DateTime a, DateTime b, bool checkTime = false)
        {
            if (checkTime)
            {
                return !(a.Hour == b.Hour
                    && a.Minute == b.Minute);
            }

            return !(a.Day == b.Day
                    && a.Month == b.Month
                    && a.Year == b.Year);
        }
    }
}
