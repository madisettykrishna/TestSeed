﻿// <auto-generated/>
using System;
using Xamarin.Forms;

namespace SeedApp.Controls
{
    public class CustomDateEntry : Entry
    {
        public static BindableProperty AllDayEventEnabledProperty =
            BindableProperty.Create<CustomDateEntry, bool>(o => o.AllDayEventEnabled, default(bool));

        public static BindableProperty SelectedDateTimeProperty =
            BindableProperty.Create<CustomDateEntry, DateTime>(o => o.SelectedDateTime, default(DateTime));

        public bool AllDayEventEnabled
        {
            get { return (bool)GetValue(AllDayEventEnabledProperty); }
            set { SetValue(AllDayEventEnabledProperty, value); }
        }

        public DateTime SelectedDateTime
        {
            get { return (DateTime)GetValue(SelectedDateTimeProperty); }
            set { SetValue(SelectedDateTimeProperty, value); }
        }
    }
}
