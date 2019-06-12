﻿// <auto-generated/>
using System.Collections;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;

namespace SeedApp.Controls
{
    public class InfiniteListView : ListView
    {
        public static readonly BindableProperty LoadMoreCommandProperty = BindableProperty.Create<InfiniteListView, ICommand>(bp => bp.LoadMoreCommand, default(ICommand));
        public static BindableProperty IsSeparatorProperty = BindableProperty.Create<ListView, SeparatorVisibility>(o => o.SeparatorVisibility, SeparatorVisibility.None, propertyChanged: OnIsVisibleChanged);

        public InfiniteListView() : base(ListViewCachingStrategy.RecycleElement)
        {
            if (Device.RuntimePlatform == Device.iOS)
            {
                SeparatorVisibility = SeparatorVisibility.None;
            }

            ItemAppearing += InfiniteListView_ItemAppearing;
        }

        public ICommand LoadMoreCommand
        {
            get { return (ICommand)GetValue(LoadMoreCommandProperty); }
            set { SetValue(LoadMoreCommandProperty, value); }
        }

        public SeparatorVisibility IsSeparator
        {
            get
            {
                return (SeparatorVisibility)GetValue(IsSeparatorProperty);
            }

            set
            {
                SetValue(IsSeparatorProperty, value);
            }
        }

        private static void OnIsVisibleChanged(BindableObject bindable, SeparatorVisibility oldvalue, SeparatorVisibility newvalue)
        {
            var item = bindable as ListView;
            if (Device.RuntimePlatform == Device.iOS)
                item.SeparatorVisibility = newvalue;
        }

        private void InfiniteListView_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            var items = ItemsSource as IList;

            if (items != null && e.Item == items[items.Count - 1])
            {
                if (LoadMoreCommand != null && LoadMoreCommand.CanExecute(null))
                    LoadMoreCommand.Execute(null);
            }
        }
    }
}