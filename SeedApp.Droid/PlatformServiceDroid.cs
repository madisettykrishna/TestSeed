using System;
using System.Collections.Generic;
using System.Threading;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views;
using SeedApp.Common.Enums;
using SeedApp.Common.Interfaces;
using Xamarin.Forms;
using Size = SeedApp.Common.Size;

namespace SeedApp.Droid
{
    public class PlatformServiceDroid : IPlatformService
    {
        private readonly Dictionary<Size, DeviceSize> _types = new Dictionary<Size, DeviceSize>();

        public PlatformServiceDroid()
        {
        }

        public Size ScreenSize
        {
            get
            {
                IWindowManager windowManager = ApplicationInfoProvider.MainApplicationContext.GetSystemService(Context.WindowService).JavaCast<IWindowManager>();
                Display disp = windowManager.DefaultDisplay;
                var met = new DisplayMetrics();
                disp.GetMetrics(met);
                return new Size(ConvertPixelsToDp(met.WidthPixels, met.Density), ConvertPixelsToDp(met.HeightPixels, met.Density));
            }
        }

        public bool IsConnected
        {
            get
            {
                return true;
            }
        }

        public string AppVersion
        {
            get
            {
                Context context = ApplicationInfoProvider.MainApplicationContext;
                var t = context.PackageManager.GetPackageInfo(context.PackageName, 0).VersionName;
                return t;
            }
        }

        public string OsVersion
        {
            get
            {
                var t = global::Android.OS.Build.VERSION.Release;
                return t.ToString();
            }
        }

        public string DeviceName => "Droid";

        public DeviceSize DeviceSize
        {
            get
            {
                var size = ScreenSize;

                var isPortrait = size.Height > size.Width;

                foreach (var keyValue in _types)
                {
                    var isSizeEqual = false;

                    if (isPortrait)
                    {
                        isSizeEqual = keyValue.Key.Width == size.Width
                                      && keyValue.Key.Height == size.Height;
                    }
                    else
                    {
                        isSizeEqual = keyValue.Key.Width == size.Height
                                      && keyValue.Key.Height == size.Width;
                    }

                    if (isSizeEqual)
                    {
                        return keyValue.Value;
                    }
                }

                return DeviceSize.XXLarge;
            }
        }

        public void InvokeOnMainThread(Action action)
        {
            Device.BeginInvokeOnMainThread(action);
        }

        public void InvokeOnBackgroundThread(Action action)
        {
            ThreadPool.QueueUserWorkItem(data => action());
        }

        private int ConvertPixelsToDp(float pixelValue, float destiny)
        {
            return (int)(pixelValue / destiny);
        }
    }
}